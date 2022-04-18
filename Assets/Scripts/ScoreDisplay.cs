using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI scoreText;
    GameSession gameSession;
    GameManager gameManager;
    LevelConfig levelConfig;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        gameSession = FindObjectOfType<GameSession>();
        gameManager = FindObjectOfType<GameManager>();
        levelConfig = FindObjectOfType<LevelConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = gameSession.GetScore().ToString("000000");
        //Debug.Log(gameSession.GetScore().ToString());
        if (!levelConfig) { return; }
        if ((gameSession.GetScore() >= levelConfig.GetTargetScore()) && (levelConfig.GetSceneName().Contains("Level")))
        {
            gameManager.LoadNextLevel();
        }


    }
}
