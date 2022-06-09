using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;

public class ScoreDisplay : NetworkBehaviour
{
    TextMeshProUGUI scoreText;
    [SyncVar(hook = nameof(SetScore))]
    int currentScore;
    GameSession gameSession;
    GameManager gameManager;
    LevelConfig levelConfig;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = transform.Find("Score Text").GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
        gameManager = FindObjectOfType<GameManager>();
        levelConfig = FindObjectOfType<LevelConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(gameSession.GetScore().ToString());
        if (!levelConfig) return;
        CmdNextLevel();
    }

    //[Command]
    private void CmdNextLevel()
    {
        if ((gameSession.GetScore() >= levelConfig.GetTargetScore()) && (levelConfig.GetSceneName().Contains("Level")))
        {
            gameManager.LoadNextLevel();
            Debug.Log("yo n-apelez nmka");
        }
    }

    [Server]
    public void SetCurrentScore(int score)
    {
        currentScore = score;
    }

    void SetScore(int oldScore, int newScore)
    {
        scoreText.text = newScore.ToString("000000");
        Debug.Log(newScore.ToString("000000"));
    }
}
