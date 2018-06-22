using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEditor;

public class BashUtils {
    private static StringBuilder procOutput = null;

    public static void RunBashCommandInPath(string cmd, string path)
    {
        UnityEngine.Debug.Log("Cmd is " + cmd);
        UnityEngine.Debug.Log("Path is " + path);

        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = "/bin/bash";
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
    //     // UnityEngine.Debug.Log("Cmd is " + cmd);
    //     // UnityEngine.Debug.Log("Path is " + path);

    //     ProcessStartInfo processInfo = new ProcessStartInfo();
    //     processInfo.FileName = "/bin/bash";
    //     processInfo.WorkingDirectory = "/";

    //     if (path != "")
    //     {
    //         processInfo.Arguments = "-c \"cd '" + path + "' && " +
    //                                             cmd + "\"";
    //     }
    //     else
    //     {
    //         processInfo.Arguments = "-c \" " +
    //                                             cmd + "\"";
    //     }

    //     // UnityEngine.Debug.Log("process args: " + processInfo.Arguments);

    //     processInfo.UseShellExecute = false;
    //     processInfo.RedirectStandardOutput = true;
    //     processInfo.RedirectStandardError = true;

    //     Process newProcess = new Process();

    //     procOutput = new StringBuilder("");
    //     newProcess.OutputDataReceived += new DataReceivedEventHandler(OnDataReceived);       

    //     newProcess = Process.Start(processInfo);
    //     newProcess.BeginOutputReadLine();
    //     string strError = newProcess.StandardError.ReadToEnd();

    //     newProcess.WaitForExit();
    //     UnityEngine.Debug.Log("Process exited with code " + newProcess.ExitCode + "\n and errors: " + strError);
    // }

    // private static void OnDataReceived(object sender, DataReceivedEventArgs e)
    // {
    //     if(e.Data != null)                                  
    //     {
    //         UnityEngine.Debug.Log(e.Data);
    //     }
    // }

    public static void RunBashCommand(string cmd)
    {
        RunBashCommandInPath(cmd, "");
    }
}
