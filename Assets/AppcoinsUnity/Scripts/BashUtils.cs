using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEditor;

public class BashUtils {
	private static Thread runBash;
    private static StringBuilder procOutput = null;

    public static void RunCommandInPath(string terminalPath, string cmd, string path)
    {
        UnityEngine.Debug.Log("Cmd is " + cmd);
        UnityEngine.Debug.Log("Path is " + path);

        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = terminalPath;
        processInfo.WorkingDirectory = "/";

        if (path != "")
        {
            processInfo.Arguments = "-c \"cd '" + path + "' && " +
                                                cmd + "\"";
        }
        else
        {
            processInfo.Arguments = "-c \" " +
                                                cmd + "\"";
        }

        UnityEngine.Debug.Log("process args: " + processInfo.Arguments);

        processInfo.UseShellExecute = false;
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;


        Process newProcess = Process.Start(processInfo);

        string strOutput = newProcess.StandardOutput.ReadToEnd();
        string strError = newProcess.StandardError.ReadToEnd();

        newProcess.WaitForExit();
        UnityEngine.Debug.Log(strOutput);
        UnityEngine.Debug.Log("Process exited with code " + newProcess.ExitCode + "\n and errors: " + strError);
    }

    // public static void RunBashCommandInPath(string cmd, string path)
    // {
    //     ProcessStartInfo processInfo = new ProcessStartInfo();
    //     processInfo.FileName = "/bin/bash";
    //     processInfo.WorkingDirectory = "/";
	//     processInfo.Arguments = "-c \"open -n /Applications/Utilities/Terminal.app --args /Users/aptoide/Desktop/bash.sh\"";

    //     processInfo.UseShellExecute = false;

	//     Process newProcess = new Process();   
	//     newProcess.StartInfo = processInfo;
	//     newProcess.Start();
    //     newProcess.WaitForExit();

    // }

    public static void RunBashCommand(string terminalPath, string cmd)
    {
        RunCommandInPath(terminalPath, cmd, "");
    }
}
