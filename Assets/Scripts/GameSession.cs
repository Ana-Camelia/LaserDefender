using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class GameSession : NetworkBehaviour
{
    //[SyncVar(hook = nameof(SetScore))]
    int score = 0;
    //[SerializeField]
    //GameObject scoreText;

    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numberGameSessions > 1)
        {
            gameObject.SetActive(false); //dezactivam obiectul pentru a fi siguri
                                         //ca nu exista alte script-uri care incearca
                                         //sa il acceseze
            Destroy(gameObject); //distrugem acest obiect care incearca sa se suprapuna
        }
        else
        {
            DontDestroyOnLoad(gameObject); //nu distruge acest obiect care incearca sa se incarce
        }
    }

    [ServerCallback]
    public int GetScore()
    {
        return score;
    }

    [Server]
    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
        FindObjectOfType<ScoreDisplay>().SetCurrentScore(score);
        FindObjectOfType<ProgressBar>().SetCurrentScore(score);
        //Debug.Log(score);
    }

    //void SetScore(int oldScore, int newScore)
    //{
    //    FindObjectOfType<ScoreDisplay>().SetCurrentScore(newScore);
    //    Debug.Log(newScore);
    //}

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
