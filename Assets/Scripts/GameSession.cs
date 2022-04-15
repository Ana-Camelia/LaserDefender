using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    int score = 0;

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

    public int GetScore()
    {
        return score;
    }

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
