using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float moveSpeed;

    public Transform target;

    public Camera mainCamera, bigMapCamera;

    private bool bigMapActive;

    public bool isBossRoom;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if(isBossRoom)
        {
            target = PlayerController.instance.transform;
        }
    }

    void Update()
    {
        if(target != null)
        {
            //kamera ve odalarýn z ekseni ayni degil
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }

        
        if(Input.GetKeyDown(KeyCode.M) && !isBossRoom)
        {
            if(!bigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }


    }


    //Oda degisikligi icin
    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }


    //Kamera
    public void ActivateBigMap()
    {
        if(!LevelManager.instance.isPaused)
        {

            bigMapActive = true;

            bigMapCamera.enabled = true;
            mainCamera.enabled = false;

            PlayerController.instance.canMove = false;

            Time.timeScale = 0f;

            UIController.instance.mapDisplay.SetActive(false);

            UIController.instance.bigMapTest.SetActive(true);

        }
    }

    public void DeactivateBigMap()
    {
        if(!LevelManager.instance.isPaused)
        {
            bigMapActive = false;

            bigMapCamera.enabled = false;
            mainCamera.enabled = true;

            PlayerController.instance.canMove = true;

            Time.timeScale = 1f;

            UIController.instance.mapDisplay.SetActive(true);

            UIController.instance.bigMapTest.SetActive(false);

        }

    }
}
