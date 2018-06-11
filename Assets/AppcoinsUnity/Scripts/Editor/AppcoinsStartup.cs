using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class Startup
{
    public const string DEFAULT_UNITY_PACKAGE_IDENTIFIER = "com.Company.ProductName";

    static Startup()
    {
        //Check if the active platform is Android. If it isn't change it
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

        //Check if min sdk version is lower than 21. If it is, set it to 21
        if (PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel21)
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;

        //Check if the bunde id is the default one and change it if it to avoid that error
        if (PlayerSettings.applicationIdentifier == DEFAULT_UNITY_PACKAGE_IDENTIFIER)
            PlayerSettings.applicationIdentifier = "com.aptoide.appcoins";

        Debug.Log("Successfully integrated Appcoins Unity plugin!");
    }
}