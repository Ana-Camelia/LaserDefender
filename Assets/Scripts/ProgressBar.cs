using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    Slider progressBar;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponent<Slider>();
        progressBar.maxValue = FindObjectOfType<LevelConfig>().GetTargetScore();
        progressBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        progressBar.value = FindObjectOfType<GameSession>().GetScore();
    }
}
