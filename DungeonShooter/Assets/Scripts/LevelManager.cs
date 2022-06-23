using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    //Level bittikten sonra gecikme suresi
    public float waitToLoad = 4f;

    //level ismi
    public string nextLevel;

    //Pause menu
    public bool isPaused;

    //Coin
    public int currentCoins;

    //Karakter baslangic nok.
    public Transform startPoint;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Karakter baslangic nok.
        PlayerController.instance.transform.position = startPoint.position;
        PlayerController.instance.canMove = true;

        //Leveller arasi gecis
        currentCoins = CharacterTracker.instance.currentCoins;


        Time.timeScale = 1f;

        //UI Coin
        UIController.instance.cointText.text = currentCoins.ToString();
    }

    void Update()
    {
        //Pause Menuyu ekrana getirir
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }

    }

    public IEnumerator LevelEnd()
    {
        AudioManager.instance.PlayLevelWin();

        //Level bitince haraket etme
        PlayerController.instance.canMove = false;

        //Fade screeni cagir
        UIController.instance.StartFadeToBlack();

        yield return new WaitForSeconds(waitToLoad);

        //Leveller arasi gecis
        CharacterTracker.instance.currentCoins = currentCoins;
        CharacterTracker.instance.currentHealth = PlayerHealthController.instance.currentHealth;
        CharacterTracker.instance.maxHealth = PlayerHealthController.instance.maxHealth;

        SceneManager.LoadScene(nextLevel);
    }

    //Pause menu
    public void PauseUnpause()
    {
        if(!isPaused)
        {
            UIController.instance.pauseMenu.SetActive(true);

            isPaused = true;

            Time.timeScale = 0f; // oyun zamani durur, fizik vs her sey donar
        }
        else
        {
            UIController.instance.pauseMenu.SetActive(false);

            isPaused = false;

            Time.timeScale = 1f;
        }
    }

    //Coin
    public void GetCoins(int amount)
    {
        currentCoins += amount;

        UIController.instance.cointText.text = currentCoins.ToString();

    }

    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if(currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.instance.cointText.text = currentCoins.ToString();

    }

}
