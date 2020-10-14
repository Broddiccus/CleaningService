using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using AudienceNetwork;

public class UIManager : MonoBehaviour
{
    //SINGLETON VARIABLES
    public static UIManager Instance;
    //UI VARIABLES
    public GameObject StartMenu;
    public GameObject GameUI;
    public GameObject PauseMenu;
    public GameObject GameOverMenu;
    public GameObject ShopMenu;
    public GameObject DescBox;
    public GameObject LevelChanger;
    public Text description;
    public Text title;
    public ObstacleHandler obstacleStarter;
    public Image Timer;
    public Image Distance;
    //public Image Patience;
    public Text Score;
    public Text Combo;
    public Text FinalScore;
    public Text MenuMoney;
    public Text MenuDiamonds;
    //Advertisement
    public InterstitialAdScene interstitialAd;
   
    //======================================================================
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
        MenuMoneyUpdate(GameManager.Instance.score, GameManager.Instance.diamonds);
        Advertisements.Instance.Initialize();
        Debug.Log("SUCCESS!");
    }
    //======================================================================
    //UI FUNCTIONS
    public void PressStart()
    {
        GameManager.Instance.gameState = GameManager.GameState.Gameplay;
        StartMenu.SetActive(false);
        GameUI.SetActive(true);
        AudioManager.Instance.GameMusic();
        GameManager.Instance.GameReset();
        LevelHandler.Instance.LevelSetup(GameManager.Instance.currentStage);
        LevelHandler.Instance.GameStart();
        //obstacleStarter.ObjectSpawning();
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
    public void PauseGame()
    {
        GameManager.Instance.gameState = GameManager.GameState.Pause;
        PauseMenu.SetActive(true);
    }
    public void Shop()
    {
        ShopMenu.SetActive(true);
        GameOverMenu.SetActive(false);
        StartMenu.SetActive(false);
        MenuMoneyUpdate(GameManager.Instance.score, GameManager.Instance.diamonds);
        //UPDATE DIAMONDS HERE LATER WHEN THEY'RE A THING
    }
    public void ShopExit()
    {
        ShopMenu.SetActive(false);
        StartMenu.SetActive(true);
    }
    public void Resume()
    {
        GameManager.Instance.gameState = GameManager.GameState.Gameplay;
        PauseMenu.SetActive(false);
    }
    public void GameOver()
    {
        AudioManager.Instance.MenuMusic();
        GameOverMenu.SetActive(true);
        // Initiate the request to load the ad.
    }
    
    public void RestartToMenu()
    {
        GameManager.Instance.Save();
        SceneManager.LoadScene(0);
        GameManager.Instance.Load();
    }
    public void RestartToGame()
    {
        GameOverMenu.SetActive(false);
        ShopMenu.SetActive(false);
        GameUI.SetActive(true);
        AudioManager.Instance.GameMusic();
        GameManager.Instance.gameState = GameManager.GameState.Gameplay;
        GameManager.Instance.GameReset();
        //obstacleStarter.ObjectSpawning();
    }
    public void ShowDescBox(string tit, string txt)
    {
        DescBox.SetActive(true);
        title.text = tit;
        description.text = txt;
        
    }
    public void HideDescBox()
    {
        DescBox.SetActive(false);
    }
    public void LevelChange(int i)
    {
        switch (i)
        {
            case 0:
                if (GameManager.Instance.officeBought)
                    GameManager.Instance.LevelChange(GameManager.StageSet.Suburbia);
                break;
            case 1:
                if (GameManager.Instance.officeBought)
                    GameManager.Instance.LevelChange(GameManager.StageSet.Office);
                break;
        }
        
    }
    public void RevealLevelSelect()
    {
        LevelChanger.SetActive(true);
    }
    //======================================================================
    //GAMEPLAY FUNCTIONS
    public void TimerUpdate(float t)
    {
        float temp = t / GameManager.Instance.timerMax;
        Timer.fillAmount = temp;
    }
    public void DistanceUpdate(float d, float dMax)
    {
        float temp = d / dMax;
        Distance.fillAmount = temp;
    }
    //public void HealthUpdate(float h)
    //{
    //    Patience.fillAmount = h;
    //}
    public void GameScoreUpdate(float s)
    {
        Score.text = "Money: $" + System.Math.Round(s, 2);
    }
    public void ComboUpdate(float c)
    {
        Combo.text = "Combo: " + System.Math.Round(c,1);
    }
    public void FinalScoreUpdate(float s)
    {
        FinalScore.text = "Final Score: $" + System.Math.Round(s, 2);
    }
    //======================================================================
    //SHOP FUNCTIONS
    public void MenuMoneyUpdate(float m, int d)
    {
        MenuMoney.text = ": " + System.Math.Round(m, 2);
        //MenuDiamonds.text = ": " + d;
    }
    //======================================================================
}
