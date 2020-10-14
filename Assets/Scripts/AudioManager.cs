using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //SINGLETON VARIABLES
    public static AudioManager Instance;
    //GAMEPLAY VARIABLES
    public AudioSource musicPlayer;
    public AudioClip[] music;
    public AudioSource sfxPlayer;
    public AudioClip[] sfx;
    //SINGLETON INITIALIZATION
    void SingletonInit()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Awake()
    {
        SingletonInit();
    }
    private void Start()
    {
        MenuMusic();
    }
    //======================================================================
    public void MenuMusic()
    {
        musicPlayer.Stop();
        musicPlayer.clip = music[0];
        musicPlayer.Play();
    }
    public void GameMusic()
    {
        musicPlayer.Stop();
        musicPlayer.clip = music[1];
        musicPlayer.Play();
    }
    public void MoneySFX()
    {
        sfxPlayer.PlayOneShot(sfx[0], 1.0f);
    }
    public void HitSFX()
    {
        sfxPlayer.PlayOneShot(sfx[1],1.0f);
    }
    public void DashSFX()
    {
        sfxPlayer.PlayOneShot(sfx[2], 1.0f);
    }
}
