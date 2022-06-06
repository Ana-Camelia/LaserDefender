using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NetworkBehaviour
{
    [Header("Health and Death")]
    [SyncVar][SerializeField] float health = 100;
    //[SerializeField] AudioClip deathSFX;
    //[SerializeField] [Range(0, 1)] float deathSFXVolume = 1f;
    [SerializeField] GameObject explosionVFX;
    [SyncVar][SerializeField] int scoreValue;
    float durationOfExplosion = 2f;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float laserSpeed = 10f;
    //[SerializeField] AudioClip laserSFX;
    //[SerializeField] [Range(0, 1)] float laserSFXVolume = 0.5f;
    [SyncVar] float shotCounter;
    float minTimeBetweenShots = 0.5f;
    float maxTimeBetweenShots = 2f;
    Vector3 laserPadding = new Vector3(0, 0.5f, 0);

    // Start is called before the first frame update
    [Server]
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    [Server]
    // Update is called once per frame
    void Update()
    {
        if (!laserPrefab) { return; }
        CountDownAndShoot();
    }

    [Server]
    private void CountDownAndShoot()
    {
        //in fiecare frame shotCounter-ul scade cu atatea secunde cate a avut frame-ul
        shotCounter -= Time.deltaTime;

        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    [Server]
    void Fire()
    {
        GameObject laser = Instantiate(
                  laserPrefab,
                  transform.position - laserPadding,
                  Quaternion.Euler(0,0,180)) as GameObject;
        //instantiem un nou laser ca gameobject
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        //AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
        NetworkServer.Spawn(laser, connectionToClient);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    [Server]
    void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    [Server]
    void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(
            explosionVFX,
            transform.position,
            Quaternion.identity) as GameObject;
        NetworkServer.Spawn(explosion, connectionToClient);
        Destroy(explosion, durationOfExplosion);
        //AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
    }

}
