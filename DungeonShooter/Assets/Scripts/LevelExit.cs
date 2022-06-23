using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //Yeni yuklenecek level adi
    public string levelToLoad;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {        
            //LevelManager scriptindeki levelEnd fonk. cagir
            StartCoroutine(LevelManager.instance.LevelEnd());
        }
    }
}
