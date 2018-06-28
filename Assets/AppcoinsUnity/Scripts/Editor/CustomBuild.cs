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

public class CustomBuildMenuItem : EditorWindow {
    [MenuItem("Custom Build/Custom Android Build")]
    public static void CallAndroidCustomBuild()
    {
        CustomBuild buildObj = new CustomBuild();
        buildObj.ExecuteCustomBuild("android");
    }

    // [MenuItem("Custom Build/ADB Install")]
    // public static void CallADBInstall()
    // {
    //     UnixCustomBuild unixBuild = new UnixCustomBuild();

    //     string path = Application.dataPath + "/../Android";
    //     // Debug.Log("Application.dataPath is " + path);

    //     unixBuild.AdbInstall(path);
    // }

    // private static StringBuilder sortOutput = null;
    // private static UnityEvent terminalEvent = new UnityEvent();
    // private static bool finished = false;
    // [MenuItem("Custom Build/Terminal")]
    // public static void RunTerminal()
    // {
    //     // // Thread t = new Thread(CustomBuildMenuItem.Terminal);
    //     // // t.Start();
    //     // System.Threading.Tasks.Task.Run(() => Terminal());
    //     // CustomBuildMenuItem.terminalEvent.AddListener(
    //     //     delegate
    //     //     {
    //     //         UnityEngine.Debug.Log("a");
    //     //     }
    //     // );
    //     // while(!CustomBuildMenuItem.finished);

    //     Terminal();
    // }

    // public static void Terminal()
    // {
    //     Process newProcess = new Process();
    //     newProcess.StartInfo.FileName = "/bin/bash";
    //     newProcess.StartInfo.WorkingDirectory = "/";
    //     newProcess.StartInfo.Arguments = "-c \"echo hello world! && ls -a -l && seq 1 50000\"";
    //     newProcess.StartInfo.UseShellExecute = false;
    //     newProcess.StartInfo.RedirectStandardOutput = true;
    //     newProcess.StartInfo.ErrorDialog = false;
    //     sortOutput = new StringBuilder("");
    //     newProcess.OutputDataReceived += new DataReceivedEventHandler(SortOutputHandler);
    //     newProcess.Start();
    //     newProcess.BeginOutputReadLine();
    //     // string out = newProcess.StandardOutput.ReadToEnd();
    //     while(!newProcess.HasExited);
    //     CustomBuildMenuItem.finished = true;
    // }

    // private static void SortOutputHandler(object sendingProcess, 
    //         DataReceivedEventArgs outLine)
    // {
    //     // Collect the sort command output.
    //     if (!String.IsNullOrEmpty(outLine.Data))
    //     {
    //         UnityEngine.Debug.Log(outLine.Data);
    //     }
    // }
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
    public static BuildStage stage;

    protected string ANDROID_STRING = "android";
    protected string BASH_LOCATION = "/bin/bash";
    protected string CMD_LOCATION = "cmd.exe";
    private string TERMINAL_CHOOSED = null;

    public CustomBuild()
    {
        if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.MacOSX ||
            SystemInfo.operatingSystemFamily == OperatingSystemFamily.Linux)
        {
            TERMINAL_CHOOSED = BASH_LOCATION;
            gradlePath = gradleUnixPath;
        }

        else if(SystemInfo.operatingSystemFamily == OperatingSystemFamily.Windows)
        {
            TERMINAL_CHOOSED = CMD_LOCATION;
            gradlePath = gradleWindowsPath;
        }

        else {
            UnityEngine.Debug.LogError("Run Unity on a desktop OS");
        }
    }

    public void ExecuteCustomBuild(string target)
    {
        if(TERMINAL_CHOOSED != null)
        {
            ExportScenes expScenes = new ExportScenes();
            string[] scenesPath = expScenes.ScenesToString(expScenes.AllScenesToExport());
            CustomBuild.continueProcessEvent.AddListener(
                delegate 
                {
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
        string path = null;

        if(target.ToLower() == ANDROID_STRING)
        {
            ChangeStage(BuildStage.UNITY_BUILD);
            path = this.AndroidCustomBuild(scenesPath);
        }

        if(path != null)
        {
            ChangeStage(BuildStage.GRADLE_BUILD);
            Build(path);
            
            if(CustomBuild.runAdbInstall)
            {
                ChangeStage(BuildStage.ADB_INSTALL);
                AdbInstall(path);
            }
        }

        ChangeStage(BuildStage.DONE);
    }

    public void ChangeStage(BuildStage theStage) {
        stage = theStage;
        CustomBuildWindow.ForceUIUpdate();
    }

    protected string AndroidCustomBuild(string[] scenesPath)
    {
        return GenericBuild(scenesPath, null, BuildTarget.Android, BuildOptions.AcceptExternalModificationsToPlayer);
    }

    protected string GenericBuild (string[] scenesPath, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        string path = this.SelectPath();
        this.CheckIfFolderAlreadyExists(path);

        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        BuildPipeline.BuildPlayer(scenesPath, path, build_target, build_options);
        return path;
    }

    protected string SelectPath() {
        return EditorUtility.SaveFolderPanel("Save Android Project to folder", "", "");
    }

    // If folder already exists in the chosen directory delete it.
    protected void CheckIfFolderAlreadyExists(string path)
    {
        string[] folders = Directory.GetDirectories(path);

        for(int i = 0; i < folders.Length; i++)
        {
            if((new DirectoryInfo(folders[i]).Name) == PlayerSettings.productName)
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

    protected void CheckAppPath(ref string path, string AppName)
    {
        string fileName = Path.GetFileName(path);

        if (fileName == AppName)
        {
            path = Path.GetDirectoryName(path) + "/";
        }
    }

    protected void Build(string path) 
    {
        this.CheckAppPath(ref CustomBuild.gradlePath, "gradle");

        string gradleCmd = "'" + gradlePath + "gradle' build";
        string cmdPath = "'" + path + "/" + PlayerSettings.productName + "'";

        BashUtils.RunCommandInPath(TERMINAL_CHOOSED, gradleCmd, cmdPath);
        // BashUtils.RunCommandWithGUI(gradleCmd, cmdPath);
    }

    protected void AdbInstall(string path) 
    {
        this.CheckAppPath(ref CustomBuild.adbPath, "adb");

        string adbCmd = "'" + CustomBuild.adbPath + "adb' install -r './build/outputs/apk/release/" + PlayerSettings.productName + "-release.apk'";
        string cmdPath = "'" + path + "/" + PlayerSettings.productName + "'";

        BashUtils.RunCommandInPath(TERMINAL_CHOOSED, adbCmd, cmdPath);
        // BashUtils.RunCommandWithGUI(adbCmd, cmdPath);
    }
}

// Draw the window for the user select what scenes he wants to export and configure player settings.
public class CustomBuildWindow : EditorWindow
{
    public static CustomBuildWindow instance;
    private SceneToExport[] scenes;
    public Vector2 scrollViewVector = Vector2.zero;

    public static void ForceUIUpdate()
    {
        UnityEngine.Debug.Log("Paint instance " + instance );
        instance.Repaint();
    }

    //Create the custom Editor Window
    public static void CreateExportScenesWindow(SceneToExport[] openScenes)
    {
        CustomBuildWindow.instance = (CustomBuildWindow)EditorWindow.GetWindowWithRect(
            typeof(CustomBuildWindow),
            new Rect(0, 0, 600, 400),
            true,
            "Custom Build Settings"
        );

        instance.scenes = openScenes;
        instance.minSize = new Vector2(600, 400);
        instance.autoRepaintOnSceneChange = true;
        instance.Show();
    }

    public void OnInspectorUpdate()
    {
        // This will only get called 10 times per second.
        Repaint();
    }

    // Display all the scenes, a button to open 'Player Settings, one to cancel and other to confirm(continue).
    void OnGUI()
    {
        switch (CustomBuild.stage)
        {
            case CustomBuild.BuildStage.IDLE:
                CreateCustomBuildUI();
                break;
            case CustomBuild.BuildStage.UNITY_BUILD:
                GUI.Label(new Rect(5, 30, 590, 40), "building gradle project...");
                break;
            case CustomBuild.BuildStage.GRADLE_BUILD:
                GUI.Label(new Rect(5, 30, 590, 40), "Running gradle to generate APK...");
                break;
            case CustomBuild.BuildStage.ADB_INSTALL:
                GUI.Label(new Rect(5, 30, 590, 40), "Installing APK...");
                break;
            case CustomBuild.BuildStage.ADB_RUN:
                GUI.Label(new Rect(5, 30, 590, 40), "Running APK...");
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

        float scenesPartHeight = adbPartHeight + 20;
        GUI.Label(new Rect(5, scenesPartHeight, 590, 40), "Select what scenes you want to export:\n(Only scenes that are in build settings are true by default)");
        float scrollViewLength = scenes.Length * 25f;
        scenesPartHeight += 25;
        scrollViewVector = GUI.BeginScrollView(new Rect(5, scenesPartHeight, 590, 330), scrollViewVector, new Rect(0, 0, 590, scrollViewLength));
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i].exportScene = GUI.Toggle(new Rect(10, 10 + i * 20, 100, 20), scenes[i].exportScene, scenes[i].scene.name);
        }
        GUI.EndScrollView();

        if (GUI.Button(new Rect(5, 370, 100, 20), "Player Settings"))
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
        }
        if (GUI.Button(new Rect(460, 370, 60, 20), "Cancel"))
        {
            this.Close();
        }

        if (CustomBuild.gradlePath != "" && GUI.Button(new Rect(530, 370, 60, 20), "Confirm"))
        {
            CustomBuild.continueProcessEvent.Invoke();
            this.Close();
        }
    }
}


// Get all the loaded scenes and aks to the user what scenes he wants to export by 'ExportScenesWindow' class.
public class ExportScenes
{
    private SceneToExport[] scenes;

    public string[] ScenesToString(SceneToExport[] scenes) 
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

    public SceneToExport[] AllScenesToExport()
    {
        this.getAllOpenScenes();
        return this.SelectScenesToExport();
    } 

    public void getAllOpenScenes()
    {
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;    
        scenes = new SceneToExport[sceneCount];

        for(int i = 0; i < sceneCount; i++)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);

            if(scenes[i] == null) 
            {
                scenes[i] = new SceneToExport();
            }

            scenes[i].scene = scene;
            scenes[i].exportScene = scene.buildIndex != -1 ? true : false;
        }
    }

    // Open ExportScenesWindow window.
    public SceneToExport[] SelectScenesToExport() 
    {
        CustomBuildWindow.CreateExportScenesWindow(scenes);
        return scenes;
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

    private UnityEngine.SceneManagement.Scene _scene;
    public UnityEngine.SceneManagement.Scene scene
    {
        get { return _scene; }
        set { _scene = value; }
    }
}
