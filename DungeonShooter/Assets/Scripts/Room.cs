using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //Kapilar icin
    public bool closeWhenEntered;
    public GameObject[] doors;


    [HideInInspector]
    public bool roomActive;

    //Harita
    public GameObject mapHider;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OpenDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);

            closeWhenEntered = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            CameraController.instance.ChangeTarget(transform);

            //Kapilar
            if(closeWhenEntered)
            {
                foreach(GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }

            //Odayi temizleme
            roomActive = true;

            //Harita
            mapHider.SetActive(false);
        }
    }

    //Odayi temizleme
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            roomActive = false;
        }
    }
}
