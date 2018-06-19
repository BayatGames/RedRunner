using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Diagnostics;

public class CustomBuild : EditorWindow
{
    private static bool continueBuild = false;

    private static UnityEvent continueProcessEvent = new UnityEvent();

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

    // Get all the loaded scenes and aks to the user what scenes he wants to export by 'ExportScenesWindow' class.
    public class ExportScenes
    {
        private SceneToExport[] scenes;

        public static string[] GetScenesToString(SceneToExport[] scenes) 
        {
            string[] pathScenes = new string[scenes.Length];

            for(int i = 0; i < scenes.Length; i++)
            {
                pathScenes[i] = scenes[i].scene.path;
            }

            return pathScenes;
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
            ExportScenesWindow.CreateExportScenesWindow(scenes);
            CustomBuild.continueProcessEvent.Invoke();
            return scenes;
        }
    }

    // Draw the window for the user select what scenes he wants to export and configure player settings.
    public class ExportScenesWindow : EditorWindow
    {
        private SceneToExport[] scenes;
        public Vector2 scrollViewVector = Vector2.zero;

        //Create the custom Editor Window
        public static void CreateExportScenesWindow(SceneToExport[] openScenes)
        {
            ExportScenesWindow scenesWindow = (ExportScenesWindow) EditorWindow.GetWindowWithRect(
                typeof(ExportScenesWindow), 
                new Rect(0, 0, 600, 400), 
                true, 
                "Custom Build Settings"
            );

            scenesWindow.scenes = openScenes;
            scenesWindow.minSize = new Vector2(600, 400);
            scenesWindow.Show();
        }

        // Display all the scenes, a button to open 'Player Settings, one to cancel and other to confirm(continue).
        void OnGUI()
        {
            GUI.Label(new Rect(5, 5, 590, 40), "Select what scenes you want to export:\n(Only scenes that are in build settings are true by default)");
            scrollViewVector = GUI.BeginScrollView (new Rect(5, 30, 590, 330), scrollViewVector, new Rect(0, 0, 1000, 1000));
            for (int i = 0; i < scenes.Length; i++) 
            {
                scenes[i].exportScene = GUI.Toggle(new Rect(10, 10 + i * 20, 100, 20) , scenes[i].exportScene, scenes[i].scene.name);
            }
            GUI.EndScrollView();

            if(GUI.Button(new Rect(5, 370, 100, 20), "Player Settings"))
            {
                EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
            }
            if(GUI.Button(new Rect(460, 370, 60, 20), "Cancel"))
            {
                CustomBuild.continueBuild = false;
                this.Close();
            }

            if(GUI.Button(new Rect(530, 370, 60, 20), "Confirm"))
            {
                CustomBuild.continueBuild = true;
                this.Close();
            }
        }
    }

    // public class InspectorWindow : EditorWindow
    // {
    //     public static void OpenInspectorWindow()
    //     {
    //         var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");
    //         var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
    //         inspectorInstance.Show();
    //     }

    //     void OnGUI()
    //     {
    //         GUILayout.Label ("GDG Breakable Object Setup Tool", EditorStyles.boldLabel);
    //     }
    // }

    [MenuItem("Custom Build/Unix Custom Android Build")]
    public static void UnixCustomAndroidBuild()
    {
        CustomBuild.continueBuild = false;
        SceneToExport[] scenes = CustomBuild.getScenesToExport();
        continueProcessEvent.AddListener(delegate{CustomBuild.AndroidCustomBuild(scenes);});

        if(CustomBuild.continueBuild)
        {
            ProcessStartInfo ExportBuildAndRunProcess = new ProcessStartInfo();
            ExportBuildAndRunProcess.FileName = "/bin/bash";
            ExportBuildAndRunProcess.Arguments = "-c \"ls -a -l && echo HELLO WORLD\"";
            ExportBuildAndRunProcess.UseShellExecute = false;
            ExportBuildAndRunProcess.RedirectStandardOutput = true;

            Process newProcess = Process.Start(ExportBuildAndRunProcess);
            string strOutput = newProcess.StandardOutput.ReadToEnd();
            newProcess.WaitForExit();
            UnityEngine.Debug.Log(strOutput);
        }
    }

    private static SceneToExport[] getScenesToExport() 
    {
        ExportScenes exportScenes = new ExportScenes();
        exportScenes.getAllOpenScenes();
        return exportScenes.SelectScenesToExport();
    }

    private static void AndroidCustomBuild(SceneToExport[] scenes) 
    {
        BuildPlayerOptions androidBuildPlayerOptions = new BuildPlayerOptions();
        androidBuildPlayerOptions.scenes = ExportScenes.GetScenesToString(scenes);
        androidBuildPlayerOptions.locationPathName = "AndroidProject";
        androidBuildPlayerOptions.target = BuildTarget.Android;
        androidBuildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(androidBuildPlayerOptions);
    }
}
