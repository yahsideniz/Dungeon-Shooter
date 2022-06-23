using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth;
    public int maxHealth;

    //Gecici yenilmezlik
    public float damageInvincLength = 1f;
    private float invincCount;


   
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Leveller arasi gecis icin
        maxHealth = CharacterTracker.instance.maxHealth;
        currentHealth = CharacterTracker.instance.currentHealth;



        //UIController koduna erisip can barini ayarliyoruz
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();


    }

    void Update()
    {
        //Gecici yenilmezlik
        if(invincCount > 0 )
        {
            invincCount -= Time.deltaTime;

            if(invincCount <= 0)
            {
                PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, 1f);

            }
        }
    }

    public void DamagePlayer()
    {
        if(invincCount <= 0)
        {
            
            AudioManager.instance.PlaySFX(11);


            currentHealth--;

            invincCount = damageInvincLength;


            //Karakterin alfa rengini degistirdik
            PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);

            if (currentHealth <= 0)
            {
                PlayerController.instance.gameObject.SetActive(false);


                UIController.instance.deathScreen.SetActive(true);

                
                AudioManager.instance.PlayGameOver();
                AudioManager.instance.PlaySFX(8);


            }

            //UIController koduna eriþip ordan slider ve text'i düzenledik
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
        }


       
    }


    //Dash atarken hasar almamak icin
    public void MakeInvincible(float length)
    {
        invincCount = length;

        PlayerController.instance.bodySR.color = new Color(PlayerController.instance.bodySR.color.r, PlayerController.instance.bodySR.color.g, PlayerController.instance.bodySR.color.b, .5f);

    }

    //Can paketi alma
    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;


        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

}
