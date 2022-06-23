using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    public GameObject[] brokenPieces;
    public int maxPieces = 5;

    //Dropping items
    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent; // yüzde kac düsme sansi


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Smash()
    {
        Destroy(gameObject);


        //Kutu kirma sesi
        AudioManager.instance.PlaySFX(0);



        //Kac parca tahta duscek
        int piecesToDrop = Random.Range(1, maxPieces);

        for (int i = 0; i < piecesToDrop; i++)
        {
            //Kutu kirilinca random parca ciksin
            int randomPiece = Random.Range(0, brokenPieces.Length);

            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);
        }


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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Yürürken kutu kirmayalim diye, dash atarken kirsin
            if (PlayerController.instance.dashCounter > 0)
            {
                Smash();
            }
        }

        if(other.tag == "PlayerBullet")
        {
            Smash();
        }
    }


}
