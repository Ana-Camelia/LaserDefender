using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    float offset;
    float columnParityFactor; //offset aditional pentru un numar par de coloane
    int waveRows, waveColumns;
    Vector3 offsetVector;
    EnemyPathing enemyPathing;

    // Start is called before the first frame update
    IEnumerator Start() //transformam metoda Start() intr-o corutina deoarece lansarea wave-urilor este repetitiva
    {
        do
        {
            yield return StartCoroutine(SpawnAllWaves()); //lansam wave-urile
        }
        while (looping); //cat timp looping este true

    }

    IEnumerator SpawnAllWaves()
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++) //parcurgem lista de wave-uri
        {
            var currentWave = waveConfigs[waveIndex];
            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave)); //abia cand lansarea unui wave s-a terminat, trecem la urmatorul
        }
    }

    IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
    {
        SetUpWaveVariables(waveConfig);
        columnParityFactor = SetMultiplicationParityFactor(waveColumns);
        for (int enemyRowIndex = 0; enemyRowIndex < waveRows; enemyRowIndex++) //lansam atatia inamici cati am indicat in waveConfig
        {
            for (int enemyColumnIndex = 0; enemyColumnIndex < waveColumns; enemyColumnIndex++)
            {
                offsetVector = SetOffsetVector(waveConfig.GetIsVertical(),enemyColumnIndex);
                //instantiem prefab-ul ales, la pozitia 0 din traiectorie, si il intoarcem la 180 de grade pentru a fi cu fata spre jucator
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypoints()[0].transform.position + offsetVector,
                    Quaternion.Euler(new Vector3(0, 0, 180)));

                enemyPathing = newEnemy.GetComponent<EnemyPathing>();
                enemyPathing.SetWaveConfig(waveConfig); //setam waveConfig-ul in codul de enemyPathing atasat fiecarui inamic
                enemyPathing.SetWaypointOffset(offsetVector);
            }

            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns()); //asteptam un anumit timp pana la instantierea urmatorului inamic
        }
    }

    private Vector3 SetOffsetVector(bool isVertical, int enemyMultiplicationIndex)
    {
        if(isVertical)
            return (enemyMultiplicationIndex % 2 == 0) ?
                new Vector3(enemyMultiplicationIndex / 2 * offset + columnParityFactor, 0, 0) :
                new Vector3(-(enemyMultiplicationIndex / 2 + 1) * offset + columnParityFactor, 0, 0);
        else
            return (enemyMultiplicationIndex % 2 == 0) ?
                new Vector3(0, enemyMultiplicationIndex / 2 * offset + columnParityFactor, 0) :
                new Vector3(0, -(enemyMultiplicationIndex / 2 + 1) * offset + columnParityFactor, 0);
    }

    private float SetMultiplicationParityFactor(int enemyColumnIndex)
    {
        return (enemyColumnIndex % 2 == 0) ? offset / 2 : 0;
    }

    private void SetUpWaveVariables(WaveConfig waveConfig)
    {
        offset = (waveConfig.GetIsVertical()) ?
            FindObjectOfType<Player>().getCanvasXLength() / (waveConfig.GetMultiplication() + 1) :
            FindObjectOfType<Player>().getCanvasYLength() / (waveConfig.GetMultiplication() + 1);
        waveRows = waveConfig.GetEnemyNumber();
        waveColumns = waveConfig.GetMultiplication();
    }
}
