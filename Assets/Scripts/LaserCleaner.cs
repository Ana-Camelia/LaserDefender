using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCleaner : MonoBehaviour
{
    //distrugem laserele atunci cand ating fence-ul
    //functiile onTrigger functioneaza doar daca collider-ul are isTrigger = true
    //spre deosebire de onCollision, Triggerele nu au physics
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
    }

}
