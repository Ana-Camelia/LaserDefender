using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneNameDisplay : MonoBehaviour
{
    TextMeshProUGUI levelText;
    LevelConfig levelConfig;

    // Start is called before the first frame update
    void Start()
    {
        levelText = GetComponent<TextMeshProUGUI>();
        levelConfig = FindObjectOfType<LevelConfig>();
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = levelConfig.GetSceneName();
    }
}
