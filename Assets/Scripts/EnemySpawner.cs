using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    [SerializeField] int startingWave = 0;
    [SerializeField] bool looping = false;
    int offset = 5;
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
        for (int enemyRowIndex = 0; enemyRowIndex < waveConfig.GetNumberOfRows(); enemyRowIndex++) //lansam atatia inamici cati am indicat in waveConfig
        {
            for (int enemyColumnIndex = 0; enemyColumnIndex < waveConfig.GetNumberOfColumns(); enemyColumnIndex++)
            {
                //instantiem prefab-ul ales, la pozitia 0 din traiectorie, si il intoarcem la 180 de grade pentru a fi cu fata spre jucator
                var newEnemy = Instantiate(
                    waveConfig.GetEnemyPrefab(),
                    waveConfig.GetWaypoints()[0].transform.position + new Vector3(enemyColumnIndex*offset,0,0),
                    Quaternion.Euler(new Vector3(0, 0, 180)));
                newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig); //setam waveConfig-ul in codul de enemyPathing atasat fiecarui inamic
                newEnemy.GetComponent<EnemyPathing>().SetWaypointOffset(new Vector3(enemyColumnIndex * offset, 0, 0));
            }

                yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns()); //asteptam un anumit timp pana la instantierea urmatorului inamic
        }
    }

}
