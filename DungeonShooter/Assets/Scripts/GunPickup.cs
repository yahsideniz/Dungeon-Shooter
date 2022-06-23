using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{
    public Gun theGun;

    public float waitToBeCollected = 0.5f;


    void Start()
    {

    }

    void Update()
    {
        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            bool hasGun = false;

            foreach(Gun gunToCheck in PlayerController.instance.availableGuns)
            {
                if(theGun.weaponName == gunToCheck.weaponName)
                {
                    hasGun = true;
                }
            }

            if(!hasGun)
            {
                Gun gunClone = Instantiate(theGun);
                gunClone.transform.parent = PlayerController.instance.gunArm;
                gunClone.transform.position = PlayerController.instance.gunArm.position;
                gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
                gunClone.transform.localScale = Vector3.one;

                PlayerController.instance.availableGuns.Add(gunClone);
                PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count -1;
                PlayerController.instance.SwitchGun();

            }


            Destroy(gameObject);

            
            AudioManager.instance.PlaySFX(7);

        }
    }
}
