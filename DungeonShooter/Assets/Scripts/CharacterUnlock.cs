using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlock : MonoBehaviour
{
    private bool canUnlock;
    public GameObject message;

    //Kafes icindeki karakteri rastgele secme
    public CharacterSelector[] charSelects;
    private CharacterSelector playerToUnblock;

    public SpriteRenderer cagedSR; // kafes icindeki karakterin sprite renderiri

    void Start()
    {
        playerToUnblock = charSelects[Random.Range(0, charSelects.Length)];

        cagedSR.sprite = playerToUnblock.playerToSpawn.bodySR.sprite;
    }

    void Update()
    {
        //Random karakter kafes icin
        if(canUnlock)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                //Kafesten cikanlar kaydolsun
                PlayerPrefs.SetInt(playerToUnblock.playerToSpawn.name, 1);


                Instantiate(playerToUnblock, transform.position, transform.rotation);

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }

    }
}
