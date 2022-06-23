using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Mermi
    public GameObject bulletToFire;
    public Transform firePoint;

    public float timeBetweenShots; // üst üste sýkmamak icin
    private float shotCounter;

    //Canvasta silah gozukmesi icin
    public string weaponName;
    public Sprite gunUI;

    //Silah satin alma
    public int itemCost;
    public Sprite gunShopSprite;

    void Start()
    {
        
    }

    void Update()
    {
        if(PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            //Üst üste ates etmesin
            if(shotCounter > 0)
            {
                shotCounter -= Time.deltaTime;
            }
            else
            {

                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                { 
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shotCounter = timeBetweenShots;

                    AudioManager.instance.PlaySFX(12);

                }
                
            }
        }
        
    }
}
