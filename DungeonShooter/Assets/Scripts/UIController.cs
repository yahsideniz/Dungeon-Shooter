using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText, cointText;

    //Dead Screen
    public GameObject deathScreen;

    //Fade Screen
    public Image fadeScreen;
    public float fadeSpeed;
    private bool fadeToBlack, fadeOutBlack;

    //Dead Screendeki butonlar
    public new string newGameScene, mainMenuScene;

    //Pause Menu
    public GameObject pauseMenu;

    //Kamera
    public GameObject mapDisplay, bigMapTest;

    //Silah
    public Image currentGun;
    public Text gunText;

    //Boss
    public Slider bossHealthBar;


    private void Awake()
    {
        instance = this;
    }

    

    void Start()
    {
        //Fade Screen
        fadeOutBlack = true;
        fadeToBlack = false;

        currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunUI;
        gunText.text = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName;


    }

    void Update()
    {
        //Fade Screen
        if(fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed*Time.deltaTime));
            if(fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if(fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }
    }


    //Level bitiminde kullanmak icin LevelManagerda cagirdik
    public void StartFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }


    public void NewGame()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene(newGameScene);

        //Kilidi acilan karakterleri sildikten sonra coklu karakter hatasini düzeltmek icin
        Destroy(PlayerController.instance.gameObject);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;


        SceneManager.LoadScene(mainMenuScene);

        //Kilidi acilan karakterleri sildikten sonra coklu karakter hatasini düzeltmek icin
        Destroy(PlayerController.instance.gameObject);

    }

    //Pause Menu
    public void Resume()
    {
        LevelManager.instance.PauseUnpause();
    }
}
