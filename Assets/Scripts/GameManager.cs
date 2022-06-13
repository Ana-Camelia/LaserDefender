using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using TMPro;

public class GameManager : NetworkBehaviour
{
    float gameOverDelay = 3f;
    float nextLevelDelay = 0.05f;
    [SerializeField] GameObject ConnectPanel;
    [SerializeField] TMP_InputField addressInput = null;

    #region Main Menu
    public void OpenConnectPanel()
    {
        ConnectPanel.SetActive(true);
    }

    public void StartServer()
    {
        NetworkManager.singleton.StartServer();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //NetworkChangeScene("Level 1");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Connect Panel
    public void StartGame()
    {
        string address = addressInput.text;

        NetworkManager.singleton.networkAddress = address;
        NetworkManager.singleton.StartClient();

    }

    public void CloseConnectPanel()
    {
        ConnectPanel.SetActive(false);
    }

    #endregion

    #region Changing Scenes
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
    #endregion

    #region Other
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
    #endregion








}
