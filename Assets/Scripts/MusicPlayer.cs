using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    void SetUpSingleton()
    {
        //GetType preia tipul clasei in care ne aflam, adica Music Player
        if(FindObjectsOfType(GetType()).Length > 1) //daca exista mai mult de 1 music player
        {
            Destroy(gameObject); //distrugem acest obiect care incearca sa se suprapuna
        }
        else
        {
            DontDestroyOnLoad(gameObject); //nu distruge acest obiect care incearca sa se incarce
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
