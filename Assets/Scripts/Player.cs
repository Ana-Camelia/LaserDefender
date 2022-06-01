using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class Player : NetworkBehaviour
{
    //[SyncVar(hook = nameof(SetMoveSpeed))]
    [SerializeField] private float moveSpeed = 20f;
    Vector2 movement;
    float lerpRate = 15;
    float paddingBottom = 1f, paddingLeftRightUp = 2f;
    float xMin, xMax;
    float yMin, yMax;

    [Header("Health")]
    [SerializeField] int health = 300;

    [Header("Explosion")]
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] [Range(0, 1)] float explosionSFXVolume = 1f;
    [SerializeField] GameObject explosionVFX;
    float durationOfExplosion = 2f;
    ShakeEffect shakeEffect;

    [Header("Laser")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] AudioClip laserSFX;
    [SerializeField] [Range(0, 1)] float laserSFXVolume = 0.5f;
    float laserSpeed = 20f;
    float laserFiringPeriod = 0.1f;
    Vector3 laserPadding = new Vector3(0, 0.5f, 0);
    Coroutine firingCoroutine;

    [Header("Materials")]
    [SerializeField] Material fullHealthMaterial;
    [SerializeField] Material mediumHealthMaterial;
    [SerializeField] Material lowHealthMaterial;

    Animator myAnimator;

    void Start()
    {
        myAnimator = GetComponent<Animator>();
        shakeEffect = Camera.main.GetComponent<ShakeEffect>();
        SetUpBoundaries();
    }

#region Screen Boundaries
    void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main; //accesam camera principala

        //determinam dimensiunile camerei
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + paddingLeftRightUp;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - paddingLeftRightUp;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + paddingBottom;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - paddingLeftRightUp;
    }

    public float GetCanvasXLength()
    {
        return xMax - xMin;
    }

    public float GetCanvasYLength()
    {
        return yMax - yMin;
    }
#endregion

    [ClientCallback] // client
    void Update()
    {
        if (!hasAuthority) { return; }
        Move();
        //CmdMove(movement);
        Fire();
    }

#region Movement and Tilting
    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }

    [ClientCallback]
    void Move()
    {
        //cate unitati s-a deplasat pe X/Y intr-un frame
        //inmultit cu cate secunde a durat frame-ul
        //astfel deplasarea va fi independenta de framerate
        var deltaX = movement.x * Time.deltaTime * moveSpeed;
        var deltaY = movement.y * Time.deltaTime * moveSpeed;

        //Mathf.Clamp verifica daca primul nr se incadreaza intre min si max
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);
        TiltPlayer(deltaX);
        //movement = new Vector2(newXPos, newYPos);
        //transform.position = movement;
        Vector3 delta = new Vector3(newXPos,newYPos,0);
        //transform.position = delta;
        CmdMove(delta);
    }

    [Server]
    public void SetMoveSpeed(float oldMoveSpeed, float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    [Command] //server
    private void CmdMove(Vector3 position)
    {
        transform.position = Vector3.Lerp(transform.position, 
                                            position,
                                            Time.deltaTime * lerpRate);
    }

    [Command]
    private void TiltPlayer(float deltaX)
    {
        if (deltaX > 0)
        {
            myAnimator.SetBool("isTiltingLeft", false);
            myAnimator.SetBool("isTiltingRight", true);
        }
        else if (deltaX < 0)
        {
            myAnimator.SetBool("isTiltingLeft", true);
            myAnimator.SetBool("isTiltingRight", false);
        }
        else
        {
            myAnimator.SetBool("isTiltingLeft", false);
            myAnimator.SetBool("isTiltingRight", false);
        }
    }
    #endregion

#region Firing
    [ClientCallback]
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
            SpawnLaser();
            AudioSource.PlayClipAtPoint(laserSFX, Camera.main.transform.position, laserSFXVolume);
            yield return new WaitForSeconds(laserFiringPeriod);
        }
    }

    [Command]
    private void SpawnLaser()
    {
        GameObject laser = Instantiate(
                          laserPrefab,
                          transform.position + laserPadding,
                          Quaternion.identity) as GameObject;

        //instantiem un nou laser ca gameobject
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        NetworkServer.Spawn(laser, connectionToClient);
    }
#endregion

#region Health and Death
    void OnTriggerEnter2D(Collider2D other)
    {
        myAnimator.SetBool("wasHitted", true);
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if(!damageDealer) { return; }
        shakeEffect.StartShaking();
        ProcessHit(damageDealer);
        StartCoroutine(AnimationWait());
    }

    IEnumerator AnimationWait()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetBool("wasHitted", false);
    }

    void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionSFXVolume);
        CheckHealth();
    }

    private void CheckHealth()
    {
        MeshRenderer bodyRenderer = transform.Find("Body").GetComponent<MeshRenderer>();

        if(health >300)
        {
            health = 300;
            bodyRenderer.material = fullHealthMaterial;
        }
        else if(health > 200)
        {
            bodyRenderer.material = fullHealthMaterial;
        }
        else if (health > 100)
        {
            bodyRenderer.material = mediumHealthMaterial;
        }
        else if (health > 0)
        {
            bodyRenderer.material = lowHealthMaterial;
        }
        else if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<GameManager>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(
            explosionVFX,
            transform.position,
            Quaternion.identity) as GameObject;
        Destroy(explosion, durationOfExplosion);
    }

    public int GetHealth()
    {
        if (health > 0) return health;
        else return 0;
    }
#endregion
}
