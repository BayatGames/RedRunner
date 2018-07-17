using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

public class CustomBuildMenuItem : EditorWindow
{

    [MenuItem("Custom Build/Custom Android Build")]
    public static void CallAndroidCustomBuild()
    {
        CustomBuild buildObj = new CustomBuild();
        buildObj.ExecuteCustomBuild("android");
    }
}

public class CustomBuild
{
    internal static UnityEvent continueProcessEvent = new UnityEvent();

    public enum BuildStage
    {
        IDLE,
        UNITY_BUILD,
        GRADLE_BUILD,
        ADB_INSTALL,
        ADB_RUN,
        DONE,
    }

    public static string gradlePath = null;
    private static string gradleWindowsPath = "C:\\Program Files\\Android\\Android Studio\\gradle\\gradle-4.4\\bin\\gradle";
    private static string gradleUnixPath = "/Applications/Android Studio.app/Contents/gradle/gradle-4.4/bin/";
    public static string adbPath = EditorPrefs.GetString("AndroidSdkRoot") + "/platform-tools/adb";
    public static bool runAdbInstall = false;
    public static bool runAdbRun = false;
    public static bool debugMode = false;
    // public static string mainActivityPath = "com.unity3d.player.UnityPlayerActivity";
    public static string mainActivityPath = ".UnityPlayerActivity";
    public static BuildStage stage;

    protected string ANDROID_STRING = "android";
    protected string BASH_LOCATION = "/bin/bash";
    protected string CMD_LOCATION = "cmd.exe";
    private string TERMINAL_CHOSEN = null;

    private string _buildPath;

    public CustomBuild()
    {
        StateBuildIdle();
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ||
            SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
        {
            TERMINAL_CHOSEN = BASH_LOCATION;
            gradlePath = gradleUnixPath;
        }

        else if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
        {
            TERMINAL_CHOSEN = CMD_LOCATION;
            gradlePath = gradleWindowsPath;
        }

        else
        {
            UnityEngine.Debug.LogError("Please run Unity on a desktop OS");
        }
    }

    public void ExecuteCustomBuild(string target)
    {
        if (TERMINAL_CHOSEN != null)
        {
            ExportScenes expScenes = new ExportScenes();
            expScenes.AllScenesToExport();
            CustomBuild.continueProcessEvent.RemoveAllListeners();
            CustomBuild.continueProcessEvent.AddListener(
                delegate
                {
                    string[] scenesPath = expScenes.ScenesToString();
                    this.ExportAndBuildCustomBuildTarget(target, scenesPath);
                }
            );
        }

        else
        {
            return;
        }
    }

    protected void ExportAndBuildCustomBuildTarget(string target, string[] scenesPath)
    {
        _buildPath = null;
        if (target.ToLower() == ANDROID_STRING)
        {
            StateUnityBuild();
            _buildPath = this.AndroidCustomBuild(scenesPath);
        }

        if (_buildPath != null)
        {
            StateGradleBuild();
            Build(_buildPath, (int retCode) =>
            {

                UnityEngine.Debug.Log("Gradle finished with retcode " + retCode);

                if (retCode == 0)
                {
                    //Check if the user marked the build to be auto installed on the device
                    if (CustomBuild.runAdbInstall)
                    {
                        StateAdbInstall();

                        AdbInstall(_buildPath, (int returnCode) =>
                        {

                            UnityEngine.Debug.Log("adb install finished with returnCode " + returnCode);

                            if (returnCode == 0)
                            {
                                if (CustomBuild.runAdbRun)
                                {
                                    StateAdbRun();

                                    AdbRun(_buildPath, (int rCode) =>
                                    {

                                        UnityEngine.Debug.Log("adb run finished with returnCode " + rCode);

                                        if (rCode == 0)
                                        {
                                            StateBuildDone();
                                        }
                                        else
                                        {
                                            StateBuildFailed("Error running build (ADB). For more information open ProcessLog.out located in projectRoot/Assets/AppcoinsUnity/Tools");
                                        }

                                    });

                                }
                                else
                                {
                                    StateBuildDone();
                                }
                            }
                            else
                            {
                                StateBuildFailed("Error installing build (ADB). For more information open ProcessLog.out located in projectRoot/Assets/AppcoinsUnity/Tools");
                            }

                        });

                    }
                    else
                    {
                        StateBuildDone();
                    }
                }
                else
                {
                    StateBuildFailed("Error building (Gradle). For more information open ProcessLog.out located in projectRoot/Assets/AppcoinsUnity/Tools");
                }

            });
        }
        else
        {
            StateBuildFailed("Error building (Unity)");
        }

    }

    #region State Handling

    private void ChangeStage(BuildStage theStage)
    {
        stage = theStage;
    }

    public void StateBuildIdle()
    {
        ChangeStage(BuildStage.IDLE);
    }

    public void StateUnityBuild()
    {
        ChangeStage(BuildStage.UNITY_BUILD);
    }

    public void StateGradleBuild()
    {
        ChangeStage(BuildStage.GRADLE_BUILD);
    }

    public void StateAdbInstall()
    {
        ChangeStage(BuildStage.ADB_INSTALL);
    }

    public void StateAdbRun()
    {
        ChangeStage(BuildStage.ADB_RUN);
    }

    public void StateBuildDone()
    {
        ChangeStage(BuildStage.DONE);

        if (_buildPath != null)
            EditorUtility.DisplayDialog("Custom Build", "Build Done!", "OK");
    }

    public void StateBuildFailed(string errorMsg)
    {
        ChangeStage(BuildStage.IDLE);

        EditorUtility.DisplayDialog("Custom Build", "Build Failed!\n" + errorMsg, "OK");
    }

    #endregion

    protected string AndroidCustomBuild(string[] scenesPath)
    {
        return GenericBuild(scenesPath, null, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    public static string GetProjectPath()
    {
        string projPath = Application.dataPath;

        int index = projPath.LastIndexOf('/');
        projPath = projPath.Substring(0, index);

        return projPath;
    }

    protected string GenericBuild(string[] scenesPath, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        string path = this.SelectPath();

        if (path == null || path.Length == 0)
        {
            return null;
        }

        string projPath = CustomBuild.GetProjectPath();

        if (path == projPath)
        {
            EditorUtility.DisplayDialog("Custom Build", "Please pick a folder that is not the project root", "Got it");
            return null;
        }

        this.DeleteIfFolderAlreadyExists(path);

        #if UNITY_5_6_OR_NEWER
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        #else
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);

        #endif

        BuildPipeline.BuildPlayer(scenesPath, path, build_target, build_options);
        return path;
    }

    protected string SelectPath()
    {
        return EditorUtility.SaveFolderPanel("Save Android Project to folder", "", "");
    }

    // If folder already exists in the chosen directory delete it.
    protected void DeleteIfFolderAlreadyExists(string path)
    {
        string[] folders = Directory.GetDirectories(path);

        for (int i = 0; i < folders.Length; i++)
        {
            if ((new DirectoryInfo(folders[i]).Name) == PlayerSettings.productName)
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folders[i]);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }
    }

    //If path for app contains appName remove it
    protected void FixAppPath(ref string path, string AppName)
    {
        string fileName = Path.GetFileName(path);

        if (fileName == AppName)
        {
            path = Path.GetDirectoryName(path) + "/";
        }
    }

    protected void Build(string path, System.Action<int> onDoneCallback)
    {
        this.FixAppPath(ref CustomBuild.gradlePath, "gradle");

        string gradleCmd = "'" + gradlePath + "gradle'";
        string gradleArgs = "build";
        string cmdPath = "'" + path + "/" + PlayerSettings.productName + "'";

        if(CustomBuild.debugMode)
        {
            gradleArgs += " --debug";
        }

        Terminal terminal = null;
        if (TERMINAL_CHOSEN == CMD_LOCATION)
        {
            terminal = new CMD();
        }

        else
        {
            terminal = new Bash();
        }

        //If we're not in windows we need to make sure that the gradle file has exec permission
        //and if not, set them
        if (SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ||
            SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
        {
            string chmodCmd = "chmod";
            string chmodArgs = "+x '" + gradlePath + "gradle'";

            terminal.RunCommand(0, chmodCmd, chmodArgs, ".", false, (int retCode) =>
            {
                if (retCode == 0)
                {
                    terminal.RunCommand(1, gradleCmd, gradleArgs, cmdPath, CustomBuild.debugMode, onDoneCallback);
                }
                else
                {
                    onDoneCallback.Invoke(-1);
                }
            });

        }
        else
        {
            terminal.RunCommand(1, gradleCmd, gradleArgs, cmdPath, CustomBuild.debugMode, onDoneCallback);
        }
    }

    //Runs overriding ADB install process
    protected void AdbInstall(string path, System.Action<int> onDoneCallback)
    {
        this.FixAppPath(ref CustomBuild.adbPath, "adb");

        string adbCmd = "'" + CustomBuild.adbPath + "adb'";
        string adbArgs = "-d install -r './build/outputs/apk/release/" + PlayerSettings.productName + "-release.apk'";
        string cmdPath = "'" + path + "/" + PlayerSettings.productName + "'";

        Terminal terminal = null;
        if (TERMINAL_CHOSEN == CMD_LOCATION)
        {
            terminal = new CMD();
        }

        else
        {
            terminal = new Bash();
        }

        terminal.RunCommand(2, adbCmd, adbArgs, cmdPath, false, onDoneCallback);
    }

    protected void AdbRun(string path, System.Action<int> onDoneCallback)
    {
        this.FixAppPath(ref CustomBuild.adbPath, "adb");

        string adbCmd = "'" + CustomBuild.adbPath + "adb'";
#if UNITY_5_6_OR_NEWER
        string adbArgs = "shell am start -n '" + PlayerSettings.applicationIdentifier + "/" + CustomBuild.mainActivityPath + "'";
#else
        string adbArgs = "shell am start -n '" + PlayerSettings.bundleIdentifier + "/" + CustomBuild.mainActivityPath + "'";
#endif
        string cmdPath = "'" + path + "/" + PlayerSettings.productName + "'";

        Terminal terminal = null;
        if (TERMINAL_CHOSEN == CMD_LOCATION)
        {
            terminal = new CMD();
        }

        else
        {
            terminal = new Bash();
        }

        terminal.RunCommand(2, adbCmd, adbArgs, cmdPath, false, onDoneCallback);
    }
}

// Draw the window for the user select what scenes he wants to export and configure player settings.
public class CustomBuildWindow : EditorWindow
{
    public static CustomBuildWindow instance;
    private SceneToExport[] scenes;
    public Vector2 scrollViewVector = Vector2.zero;

    //Create the custom Editor Window
    public static void CreateExportScenesWindow(ref SceneToExport[] openScenes)
    {
        CustomBuildWindow.instance = (CustomBuildWindow)EditorWindow.GetWindowWithRect(
            typeof(CustomBuildWindow),
            new Rect(0, 0, 600, 500),
            true,
            "Custom Build Settings"
        );

        instance.scenes = openScenes;
        instance.minSize = new Vector2(600, 500);
        instance.autoRepaintOnSceneChange = true;
        instance.Show();
    }

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }

    void OnGUI()
    {
        switch (CustomBuild.stage)
        {
            case CustomBuild.BuildStage.IDLE:
                CreateCustomBuildUI();
                break;
            case CustomBuild.BuildStage.UNITY_BUILD:
                GUI.Label(new Rect(5, 30, 590, 40), "building gradle project...\nPlease be patient as Unity might stop responding...\nThis process will launch external windows so that you can keep tracking the build progress");
                break;
            case CustomBuild.BuildStage.GRADLE_BUILD:
                GUI.Label(new Rect(5, 30, 590, 40), "Running gradle to generate APK...\nPlease be patient...");
                break;
            case CustomBuild.BuildStage.ADB_INSTALL:
                GUI.Label(new Rect(5, 30, 590, 40), "Installing APK...\nPlease be patient...");
                break;
            case CustomBuild.BuildStage.ADB_RUN:
                GUI.Label(new Rect(5, 30, 590, 40), "Running APK...\nPlease be patient...");
                break;
            case CustomBuild.BuildStage.DONE:
                this.Close();
                break;
        }
    }

    void CreateCustomBuildUI()
    {
        float gradlePartHeight = 5;
        GUI.Label(new Rect(5, gradlePartHeight, 590, 40), "Select the gradle path");
        gradlePartHeight += 20;
        CustomBuild.gradlePath = GUI.TextField(new Rect(5, gradlePartHeight, 590, 20), CustomBuild.gradlePath);

        float adbPartHeight = gradlePartHeight + 20;
        GUI.Label(new Rect(5, adbPartHeight, 590, 40), "Select the adb path");
        adbPartHeight += 20;
        CustomBuild.adbPath = GUI.TextField(new Rect(5, adbPartHeight, 590, 20), CustomBuild.adbPath);
        adbPartHeight += 20;
        CustomBuild.runAdbInstall = GUI.Toggle(new Rect(5, adbPartHeight, 590, 20), CustomBuild.runAdbInstall, "Install build when done?");

        float adbRunPartHeight = adbPartHeight + 20;
                                                                                    // com.unity3d.player.UnityPlayerActivity
        GUI.Label(new Rect(5, adbRunPartHeight, 590, 40), "Path to the main activity name (.UnityPlayerActivity by default)");
        adbRunPartHeight += 20;
        CustomBuild.mainActivityPath = GUI.TextField(new Rect(5, adbRunPartHeight, 590, 20), CustomBuild.mainActivityPath);
        adbRunPartHeight += 20;
        CustomBuild.runAdbRun = GUI.Toggle(new Rect(5, adbRunPartHeight, 590, 20), CustomBuild.runAdbRun, "Run build when done?"); 

        float debugModeHeight = adbRunPartHeight + 20;
        CustomBuild.debugMode = GUI.Toggle(new Rect(5, debugModeHeight, 590, 20), CustomBuild.debugMode, "Run gradle in debug mode? This will not end gradle terminal automatically.");

        float scenesPartHeight = debugModeHeight + 20;
        GUI.Label(new Rect(5, scenesPartHeight, 590, 40), "Select what scenes you want to export:\n(Only scenes that are in build settings are true by default)");
        float scrollViewLength = scenes.Length * 25f;
        scenesPartHeight += 30;
        scrollViewVector = GUI.BeginScrollView(new Rect(5, scenesPartHeight, 590, 215), scrollViewVector, new Rect(0, 0, 500, scrollViewLength));
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].exportScene = GUI.Toggle(new Rect(10, 10 + i * 20, 500, 20), scenes[i].exportScene, scenes[i].scene.path);
        }
        GUI.EndScrollView();

        if (GUI.Button(new Rect(5, 470, 100, 20), "Player Settings"))
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
        }
        if (GUI.Button(new Rect(460, 470, 60, 20), "Cancel"))
        {
            this.Close();
        }

        if (CustomBuild.gradlePath != "" && GUI.Button(new Rect(530, 470, 60, 20), "Confirm"))
        {
            CustomBuild.continueProcessEvent.Invoke();
            this.Close();
        }
    }
}


// Get all the loaded scenes and asks the user what scenes he wants to export by 'ExportScenesWindow' class.
public class ExportScenes
{
    private SceneToExport[] scenes = null;

    public string[] ScenesToString() 
    {
        ArrayList pathScenes = new ArrayList();

        for(int i = 0; i < scenes.Length; i++)
        {
            if(scenes[i].exportScene)
            {
                pathScenes.Add(scenes[i].scene.path);
            }
        }

        return (pathScenes.ToArray(typeof(string)) as string[]);
    }

    public void AllScenesToExport()
    {
        this.getAllOpenScenes();
        this.SelectScenesToExport();
    } 

    public void getAllOpenScenes()
    {
        var allScenes = EditorBuildSettings.scenes;
        int sceneCount = allScenes.Length;
        //        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;  
        scenes = new SceneToExport[sceneCount];

        for(int i = 0; i < sceneCount; i++)
        {
            //UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
            EditorBuildSettingsScene scene = allScenes[i];

            if(scenes[i] == null) 
            {
                scenes[i] = new SceneToExport();
            }

            scenes[i].scene = scene;
            scenes[i].exportScene = scene.enabled ? true : false;
        }
    }

    // Opens ExportScenesWindow window.
    public void SelectScenesToExport() 
    {
        CustomBuildWindow.CreateExportScenesWindow(ref scenes);
    }
}

// Custom class to save the loaded scenes and a bool for each scene that tells us if the user wants to export such scene or not.
public class SceneToExport
{
    private bool _exportScene;
    public bool exportScene
    {
        get { return _exportScene; }
        set { _exportScene = value; }
    }

    private EditorBuildSettingsScene _scene;
    public EditorBuildSettingsScene scene
    {
        get { return _scene; }
        set { _scene = value; }
    }
}