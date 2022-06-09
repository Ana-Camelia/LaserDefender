using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class LevelConfig : NetworkBehaviour
{
    [SerializeField] int targetScore = 100;
    int sceneIndex;
    string sceneName;

    public int GetTargetScore()
    {
        return targetScore;
    }

    public int GetSceneIndex()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        return sceneIndex;
    }

    public string GetSceneName()
    {
        sceneName = SceneManager.GetActiveScene().name;
        return sceneName;
    }
}
