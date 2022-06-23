using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    public AudioSource levelMusic, gameOverMusic, winMusic;

    public AudioSource[] sfx;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //game over muzigi
    //Player health kodunda canin 0 altina düstügünü belirten kodda bu fonk. cagirdik
    public void PlayGameOver()
    {
        levelMusic.Stop();

        gameOverMusic.Play();
    }

    public void PlayLevelWin()
    {
        levelMusic.Stop();

        winMusic.Play();
    }

    public void PlaySFX(int sfxToPlay)
    {
        //Kutu kirma sesi yapcaz, breakablesta cagýrdýk bu fonks.
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}
