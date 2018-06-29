using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public abstract class Terminal
{
    public ProcessStartInfo InitializeProcessInfo(string terminalPath)
    {
        ProcessStartInfo processInfo = new ProcessStartInfo();
        processInfo.FileName = terminalPath;
        processInfo.WorkingDirectory = "/";
        processInfo.UseShellExecute = false;
        processInfo.CreateNoWindow = true;
        return processInfo;
    }

    public virtual void RunCommand(string cmd, string path) {}

    public void RunTerminalCommand(string terminalPath, string cmd)
    {
        RunCommand(cmd, "");
    }
}

public class Bash : Terminal
{
    protected static string TERMINAL_PATH = "/bin/bash";

    protected virtual void RunBashCommand(string cmd, string path) {}

    public override void RunCommand(string cmd, string path)
    {
        Bash t;

        if(Directory.Exists("/Applications/Utilities/Terminal.app") || Directory.Exists("/Applications/Terminal.app"))
        {
            t = new BashGUI();
            t.RunBashCommand(cmd, path);
        }

        else
        {
            t = new BashCommandLine();
            t.RunBashCommand(cmd, path);
        }
    }
}

public class BashCommandLine : Bash
{
    protected override void RunBashCommand(string cmd, string path)
    {
        UnityEngine.Debug.Log("Cmd is " + cmd);
        UnityEngine.Debug.Log("Path is " + path);

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardError = true;

        if (path != "")
        {
            processInfo.Arguments = "-c \"cd " + path + " && " + cmd + "\"";
        }
        else
        {
            processInfo.Arguments = "-c \"" + cmd + "\"";
        }

        UnityEngine.Debug.Log("process args: " + processInfo.Arguments);

        Process newProcess = Process.Start(processInfo);

        string strOutput = newProcess.StandardOutput.ReadToEnd();
        string strError = newProcess.StandardError.ReadToEnd();

        newProcess.WaitForExit();
        UnityEngine.Debug.Log(strOutput);
        UnityEngine.Debug.Log("Process exited with code " + newProcess.ExitCode + "\n and errors: " + strError);
    }
}

public class BashGUI : Bash
{
    protected override void RunBashCommand(string cmd, string path)
    {
        CreateSHFileToExecuteCommand(cmd, path);

        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.FileName = "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal";
        processInfo.CreateNoWindow = false;

	    processInfo.Arguments = "'" + Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh'";

	    Process newProcess = new Process();   
	    newProcess.StartInfo = processInfo;
	    newProcess.Start();

        //For the process to complete we check with, 5s interval, for the existence of ProcessCompleted.out
        while(!File.Exists(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out"))
        {
            Thread.Sleep(5000);
        }

        //Now we can safely kill the process
        if(!newProcess.HasExited)
        {
            newProcess.Kill();
        }
    }

    //This creates a bash file that gets executed in the specified path
    private void CreateSHFileToExecuteCommand(string cmd, string path)
    {
        StreamWriter writer = new StreamWriter(Application.dataPath + "/AppcoinsUnity/Tools/BashCommand.sh", false);

        writer.WriteLine("#!/bin/sh");

        //Put terminal as first foreground application
        writer.WriteLine("osascript -e 'activate application \"/Applications/Utilities/Terminal.app\"'");
        writer.WriteLine("cd " + path);
        writer.WriteLine(cmd);
        writer.WriteLine("echo 'done' > '" + Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out'");
        writer.WriteLine("exit");
        // writer.WriteLine("osascript -e 'tell application \"Terminal\" to close first window'");
        writer.Close();

        File.Delete(Application.dataPath + "/AppcoinsUnity/Tools/ProcessCompleted.out");
    }
}

public class CMD : Terminal
{
    protected static string TERMINAL_PATH = "cmd.exe";
    private static bool NO_GUI = false;

    public override void RunCommand(string cmd, string path)
    {
        ProcessStartInfo processInfo = InitializeProcessInfo(TERMINAL_PATH);
        processInfo.CreateNoWindow = NO_GUI;
        processInfo.RedirectStandardInput = true;

        if (path != "")
        {
            processInfo.Arguments = "/c \"cd " + path + " && " + cmd + "\"";
        }
        else
        {
            processInfo.Arguments = "/c \"" + cmd + "\"";
        }

        // Replace string from bash fromat to cmd format
        processInfo.Arguments = processInfo.Arguments.Replace("\"", "");
        processInfo.Arguments = processInfo.Arguments.Replace("'", "\"");

        Process newProcess = Process.Start(processInfo);
        newProcess.WaitForExit();
    }
}
