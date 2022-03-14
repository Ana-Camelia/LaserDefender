using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float moveSpeed = 10f;
    float padding = 1f;

    [Header("Health")]
    [SerializeField] int health = 300;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] float deathSFXVolume = 1f;

    float xMin, xMax;
    float yMin, yMax;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.5f;
    float laserSpeed = 20f;
    float laserFiringPeriod = 0.1f;
    Vector3 laserPadding = new Vector3(0, 0.5f, 0);
    Coroutine firingCoroutine;

    Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        SetUpBoundaries();
    }

    void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main; //accesam camera principala

        //determinam dimensiunile camerei
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    void Move()
    {
        //cate unitati s-a deplasat pe X/Y intr-un frame
        //inmultit cu cate secunde a durat frame-ul
        //astfel deplasarea va fi independenta de framerate
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        //Mathf.Clamp verifica daca primul nr se incadreaza intre min si max
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
    void Fire()
    {
        if (Input.GetButtonDown("Fire1")) //left click
        {
            //incepem o rutina pentru a trage continuu cand tinem Fire1 apasat
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1")) //left click
        {
            //oprim rutina cand ridicam degetul de pe buton
            StopCoroutine(firingCoroutine);
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            GameObject laser = Instantiate(
                  laserPrefab,
                  transform.position + laserPadding,
                  Quaternion.identity) as GameObject;

            //instantiem un nou laser ca gameobject
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
            yield return new WaitForSeconds(laserFiringPeriod);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        myAnimator.SetBool("wasHitted", true);
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        ProcessHit(damageDealer);
        StartCoroutine(AnimationWait());
    }

    IEnumerator AnimationWait()
    {
        yield return new WaitForSeconds(1);
        myAnimator.SetBool("wasHitted", false);
    }
    void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<GameManager>().LoadGameOver();
        Destroy(gameObject);
        //GameObject explosion = Instantiate(
        //    explosionVFX,
        //    transform.position,
        //    Quaternion.identity) as GameObject;
        //Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
    }

    public int GetHealth()
    {
        if (health > 0) return health;
        else return 0;
    }
}
