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
        if (NetworkServer.isLoadingScene) return;
        //StartCoroutine(DelayNextLevel());
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(FindObjectOfType<LevelConfig>().GetSceneIndex() + 1);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    IEnumerator DelayNextLevel()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(nextLevelDelay);
        Time.timeScale = 1;
        string pathToScene = SceneUtility.GetScenePathByBuildIndex(FindObjectOfType<LevelConfig>().GetSceneIndex() + 1);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
        NetworkManager.singleton.ServerChangeScene(sceneName);
        //SceneManager.LoadScene(FindObjectOfType<LevelConfig>().GetSceneIndex() + 1);
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
