using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TargetScoreDisplay : MonoBehaviour
{
    TextMeshProUGUI targetScoreText;
    LevelConfig levelConfig;

    // Start is called before the first frame update
    void Start()
    {
        targetScoreText = GetComponent<TextMeshProUGUI>();
        levelConfig = FindObjectOfType<LevelConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        targetScoreText.text = levelConfig.GetTargetScore().ToString();
    }
}
