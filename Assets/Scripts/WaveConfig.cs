using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Header("Prefabs")]
    [SerializeField] GameObject enemyPrefab; //inamicul care va aparea in wave
    [SerializeField] GameObject pathPrefab;  //traiectoria pe care o urmeaza inamicul

    [Header("Spawning")]
    [SerializeField] float timeBetweenSpawns = 0.5f; //timpul de spawn intre inamicii aceluiasi wave
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] float moveSpeed = 10f; //viteza de deplasare a inamicilor din wave
    [SerializeField] int numberOfEnemies = 5; //numarul de inamici din wave
    
    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public List<Transform> GetWaypoints() 
    {
        //"copiii" traiectoriei reprezinta punctele prin care trece inamicul
        // se creeaza o lista cu copiii pathPrefab-ului
        var waveWaypoints = new List<Transform>();
        foreach(Transform child in pathPrefab.transform)
        {
            waveWaypoints.Add(child);
        }
        return waveWaypoints; 
    }
    public float GetTimeBetweenSpawns() { return timeBetweenSpawns; }
    public float GetSpawnRandomFactor() { return spawnRandomFactor; }
    public float GetMoveSpeed() { return moveSpeed; }
    public int GetNumberOfEnemies() { return numberOfEnemies; }
}
