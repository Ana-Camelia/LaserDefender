using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        BuildWindowsServer();
        BuildLinuxServer();
        BuildWindowsClient();
    }

    [MenuItem("Build/Build Server (Windows)")]
    public static void BuildWindowsServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainMenu.unity",
                                            "Assets/Scenes/Level 1.unity",
                                            "Assets/Scenes/Level 2.unity",
                                            "Assets/Scenes/Level 3.unity",
                                            "Assets/Scenes/Congrats.unity",
                                            "Assets/Scenes/GameOver.unity"  };
        buildPlayerOptions.locationPathName = "D:/Licenta/Builds/Windows/Server/Server.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Building Server (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Server (Windows).");
    }

    [MenuItem("Build/Build Server (Linux)")]
    public static void BuildLinuxServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainMenu.unity",
                                            "Assets/Scenes/Level 1.unity",
                                            "Assets/Scenes/Level 2.unity",
                                            "Assets/Scenes/Level 3.unity",
                                            "Assets/Scenes/Congrats.unity",
                                            "Assets/Scenes/GameOver.unity"  };
        buildPlayerOptions.locationPathName = "D:/Licenta/Builds/Linux/Server/Server.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Building Server (Linux)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Server (Linux).");
    }


    [MenuItem("Build/Build Client (Windows)")]
    public static void BuildWindowsClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainMenu.unity",
                                            "Assets/Scenes/Level 1.unity",
                                            "Assets/Scenes/Level 2.unity",
                                            "Assets/Scenes/Level 3.unity",
                                            "Assets/Scenes/Congrats.unity",
                                            "Assets/Scenes/GameOver.unity"  };
        buildPlayerOptions.locationPathName = "D:/Licenta/Builds/Windows/Client/Client.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

        Console.WriteLine("Building Client (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Client (Windows).");
    }
}
