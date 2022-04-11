using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    //in acest cod se descrie comportamentul
    WaveConfig waveConfig;
    List<Transform> waypoints;

    int waypointIndex = 1;
    Vector3 waypointOffset = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoints(); //preluam lista de puncte din waveConfig care reprezinta traiectoria
        //transform.position = waypoints[waypointIndex].transform.position; //pozitionam inamicul in dreptul primului waypoint din traiectorie
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    public void SetWaypointOffset(Vector3 waypointOffset)
    {
        this.waypointOffset = waypointOffset;
    }

    private void Move()
    {
        //daca inca nu am ajuns la finalul listei, continuam traiectoria
        if (waypointIndex < waypoints.Count)
        {
            var targetPosition = waypoints[waypointIndex].transform.position + waypointOffset; //pozitia la care vrem sa ajungem
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime; //distanta parcursa in acest frame
                                                //inmultind viteza cu Time.deltaTime() (adica cat a durat frame-ul)
                                                //obtinem o miscare independenta de frame rate
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame); //deplasam obiectul catre urmatorul waypoint

            if (transform.position == targetPosition) //daca am ajuns la destinatie
            {
                waypointIndex++;                        //trecem la urmatorul waypoint din traiectorie
            }
        }
        else //altfel inseamna ca traiectoria a fost parcursa, deci putem sa distrugem obiectul(inamicul)
        {
            Destroy(gameObject);
        }
    }
}
