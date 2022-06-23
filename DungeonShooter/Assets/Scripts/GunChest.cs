using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    public GunPickup[] potentialGuns;

    public SpriteRenderer SR; //Sandigin acik hali
    public Sprite chestOpen;


    public GameObject notification;

    private bool canOpen, isOpen;

    public Transform spawnPoint;

    public float scaleSpeed = 2f;

    void Start()
    {
        
    }

    void Update()
    {
        if(canOpen && !isOpen)
        {
            if(Input.GetKey(KeyCode.E))
            {
                int gunSelect = Random.Range(0, potentialGuns.Length);

                Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);

                SR.sprite = chestOpen;

                isOpen = true;

                transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }


        if(isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, Time.deltaTime * scaleSpeed);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            notification.SetActive(true);

            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(false);

            canOpen = false;
        }
    }
}
