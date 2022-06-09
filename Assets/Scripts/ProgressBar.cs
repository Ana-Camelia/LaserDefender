using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ProgressBar : NetworkBehaviour
{
    Slider progressBar;
    [SyncVar(hook = nameof(SetScore))]
    int currentScore;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = transform.Find("Level Progress").GetComponent<Slider>();
        progressBar.maxValue = FindObjectOfType<LevelConfig>().GetTargetScore();
        progressBar.value = 0;
    }

    [Server]
    public void SetCurrentScore(int score)
    {
        currentScore = score;
    }

    void SetScore(int oldScore, int newScore)
    {
        progressBar.value = newScore;
    }
}
