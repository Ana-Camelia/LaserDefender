using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float gameOverDelay = 3f;
    float nextLevelDelay = 0.05f;


    public void LoadNextLevel()
    {
        StartCoroutine(DelayNextLevel());
    }

    IEnumerator DelayNextLevel()
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSeconds(nextLevelDelay);
        Time.timeScale = 1;
        SceneManager.LoadScene(FindObjectOfType<LevelConfig>().GetSceneIndex() + 1);
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
