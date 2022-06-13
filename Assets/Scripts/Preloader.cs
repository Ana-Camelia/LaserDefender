using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preloader : MonoBehaviour
{
    void Awake()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for(int i = 0;i < args.Length;i++)
        {
            if (args[i] == "-launch-as-client")
            {
                FindObjectOfType<GameManager>().StartGame();
            }
            else if (args[i] == "-launch-as-server")
            {
                FindObjectOfType<GameManager>().StartServer();
            }
        }
    }

}
