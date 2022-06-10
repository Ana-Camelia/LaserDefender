using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : NetworkBehaviour
{
    float gameOverDelay = 3f;
    float nextLevelDelay = 0.05f;


    [Server]
    public void LoadNextLevel()
    {
        StartCoroutine(DelayNextLevel());
    }

    IEnumerator DelayNextLevel()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(FindObjectOfType<LevelConfig>().GetSceneIndex() + 1);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
        NetworkChangeScene(sceneName);
    }

    [Server]
    private static void NetworkChangeScene(string sceneName)
    {
        if (NetworkServer.isLoadingScene) return;
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }



    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(DelayGameOver());
    }

    IEnumerator DelayGameOver()
    {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene("GameOver");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
