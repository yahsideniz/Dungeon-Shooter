using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody2D RB;
    public float moveSpeed;


    [Header("Chase Player")]
    public bool shouldChasePlayer;
    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    /* --------------------------------- */

    //Düsmanlar
    [Header("Run Away")]
    public bool shouldRunAway;
    public float runawayRange;


    //Düsmanlar
    [Header("Wandering")]
    public bool shouldWander;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    //Düsmanlar
    [Header("Patrolling")]
    public bool shouldPatrol;
    public Transform[] patrolPoints;
    private int currentPatrolPoint;

    /* --------------------------------- */

    

    //Enemy Fire
    [Header("Shooting")]
    public bool shouldShoot;


    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;

    public float shootRange;


    [Header("Variables")]
    public SpriteRenderer Body;
    public Animator anim;

    //Can sistemi
    public int health = 150;

    //Olum efekti
    public GameObject[] deathSplatters; // birden cok olmasini isteme nedenimiz her olunce ayný efekt olmasýn diye

    //Hit effect
    public GameObject hitEffect;

    //Dropping items
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent; // yüzde kac düsme sansi



    void Start()
    {
        //Dusmanlardan bazilari oyun basinda rastgele bir yere gitmesi icin
        if(shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);

        }
    }

    void Update()
    {
        if(Body.isVisible && PlayerController.instance.gameObject.activeInHierarchy) // Oyun ekranýnda gözüküyorsa
        {
            moveDirection = Vector3.zero;

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer) // 2 nokta arasý mesafe icin vector3.distance
            {
                moveDirection = PlayerController.instance.transform.position - transform.position;
            }
            else
            {
                
                if(shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection;


                        if(wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    if(pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;

                        if(pauseCounter <= 0)
                        {
                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);


                            wanderDirection = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f);
                        
                        
                        }
                    }
                }

                
                if(shouldPatrol)
                {
                    moveDirection = patrolPoints[currentPatrolPoint].position - transform.position;

                    if (Vector3.Distance(transform.position, patrolPoints[currentPatrolPoint].position) < .2f)
                    {
                        currentPatrolPoint++;
                        if(currentPatrolPoint >= patrolPoints.Length)
                        {
                            currentPatrolPoint = 0;
                        }
                    }
                
                }

            }


            
            if (shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {
                moveDirection = transform.position - PlayerController.instance.transform.position;
            }
            



            moveDirection.Normalize();

            RB.velocity = moveDirection * moveSpeed;



            //Enemy Fire
            if(shouldShoot && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < shootRange)
            {
                fireCounter -= Time.deltaTime;

                if(fireCounter <= 0 )
                {
                    fireCounter = fireRate;
                    Instantiate(bullet, firePoint.position, firePoint.rotation);

                    AudioManager.instance.PlaySFX(13);

                }
            }

        }
        else //Karakter yok oldugunda, dusman dursun
        {
            RB.velocity = Vector2.zero;
        }


        //Animasyon
        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);

        }

    }

    public void DamageEnemy(int damage)
    {
        health -= damage;

        AudioManager.instance.PlaySFX(2);


        //Hit effect
        Instantiate(hitEffect, transform.position, transform.rotation);

        if(health <= 0)
        {
            Destroy(gameObject);

            AudioManager.instance.PlaySFX(1);

            int selectedSplatter = Random.Range(0, deathSplatters.Length); // Virgulden sonra sayi yazmadik cünkü random secimde en buyuk olaný hic secmez

            int rotation = Random.Range(0, 4);


            Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90f)); // karakter yok olduktan sonra cýkan splatterlar farký rotasyonlarda cýksýn


            //Drop items
            if (shouldDropItem)
            {
                float dropChance = Random.Range(0f, 100f);

                if (dropChance < itemDropPercent)
                {
                    int randomItem = Random.Range(0, itemsToDrop.Length);

                    Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
                }
            }
        }
    }


}
