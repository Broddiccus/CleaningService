using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    //SINGLETON VARIABLES
    public static GameManager Instance;
    //GAMEPLAY VARIABLES
    //scene and transition variables
    public enum GameState { StartMenu, Pause, Gameplay, GameOver, Shop };
    public enum StageSet { Suburbia, Office, Space };
    
    [HideInInspector] public GameState gameState = GameState.StartMenu;
    public StageSet currentStage = StageSet.Suburbia;
    public bool officeBought = false;
    [HideInInspector]
    public bool transitionStage = false;
    [HideInInspector] public event Action<bool> onTransitionStageChange;
    [HideInInspector] public event Action onGameRestart;
    public Animator playerCon;
    public int gameOverNumber;
    public ShopItem[] shopItems;
    //gameplay values
    public int diamonds;
    public float score; //(score)
    public float service = 1.0f;
    [HideInInspector] public float gameScore;
    public GameObject CoinPrefab;
    [HideInInspector] public float comboMulitplier = 0.01f; //(combo)
    private float combo = 1.0f;
    [HideInInspector] public bool comboGain = false;
    public float comboMax = 3.0f;
    public float paidMulti = 0.0f;
    private float timer; //(timer)
    public float timerMax = 120.0f;
    private float distance = 0.0f; //(distance)
    [HideInInspector] public float curDisMax;
    public float distanceMax = 100.0f;
    public float transitionDisMax = 30.0f;
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
        timer = timerMax;
        curDisMax = distanceMax;
    }
    private void Start()
    {
        Load();
    }
    private void OnDisable()
    {
        Save();
    }
    //======================================================================
    //GAMEPLAY FUNCTIONS
    private void FixedUpdate()
    {
        if (combo > 1 && !comboGain)
        {
            combo -= Time.deltaTime;
        }
        if (gameState == GameState.Gameplay)
        {
            timer -= Time.deltaTime;
            distance += LevelHandler.Instance.scrollingSpeed * Time.deltaTime;
            UIManager.Instance.TimerUpdate(timer);
            UIManager.Instance.DistanceUpdate(distance, curDisMax);
            UIManager.Instance.GameScoreUpdate(gameScore);
            UIManager.Instance.ComboUpdate(combo);
            if (timer <= 0)
            {
                score += gameScore;
                gameState = GameState.GameOver;
                UIManager.Instance.GameOver();
                GameReset();
            }
            if (distance >= curDisMax)
            {
                distance = 0;
                transitionStage = !transitionStage;
                LevelHandler.Instance.transitioning = true;
                switch (transitionStage)
                {
                    case true:
                        curDisMax = transitionDisMax;
                        break;
                    case false:
                        curDisMax = distanceMax;
                        break;
                }
                if (onTransitionStageChange != null)
                    onTransitionStageChange(transitionStage);

            }
        }
    }
    public void HealthChange(float h)
    {
        AudioManager.Instance.HitSFX();
        timer += h;
        //UIManager.Instance.HealthUpdate(health);
        PlayerScript.Instance.DamageAnim();
        combo -= 0.5f;
        if (combo < 1)
        {
            combo = 1;
        }
    }
    public void GameReset()
    {
        score += gameScore;
        UIManager.Instance.FinalScoreUpdate(gameScore);
        UIManager.Instance.MenuMoneyUpdate(score, diamonds);
        gameOverNumber++;
        if (gameOverNumber >= 3)
        {
            ShowInterstitial();
            gameOverNumber = 0;
        }
        LevelHandler.Instance.GameEnd();
        timer = timerMax;
        distance = 0.0f;
        gameScore = 0.00f;
        combo = 1.0f;
        transitionStage = false;
        
        if (onTransitionStageChange != null)
            onTransitionStageChange(transitionStage);
        if (onGameRestart  != null)
            onGameRestart();
    }
    public float ScoreGain(float x)
    {
        AudioManager.Instance.MoneySFX();
        if (combo < comboMax)
        {
            combo += comboMulitplier;
        }
        else
        {
            combo = comboMax;
        }
        gameScore += x * (combo + paidMulti);
        return x * combo;
    }
    public void PowerupBuy(string n, float i) //ADD TO THIS WHEN YOU THINK UP A NEW POWERUP TO ADD THE FUNCTIONALITY
    {
        switch (n)
        {
            case "Shoes":
                PlayerScript.Instance.speed += i;
                break;
            case "Fitness DVDS":
                timerMax += i;
                break;
            case "Vacuum Power":
                PlayerScript.Instance.ChangeColliderScale(i);
                break;
            case "Painters Pants":
                PlayerScript.Instance.dashMoveSpeed += i;
                break;
            case "Vacuum Manual":
                paidMulti += i;
                break;
            case "Customer Service Training":
                LevelHandler.Instance.speedIncval += i;
                break;
            case "Office Contract":
                officeBought = true;
                UIManager.Instance.RevealLevelSelect();
                LevelChange(StageSet.Office);
                break;
        }
    }
    public void fixInteract()
    {
        timer += service * 10.0f;
    }
    public void ShowInterstitial()
    {
        Advertisements.Instance.ShowInterstitial();
    }
    public void LevelChange(StageSet s)
    {
        currentStage = s;
        LevelHandler.Instance.LevelSetup(currentStage);
    }
    //======================================================================
    //SAVE/LOAD
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveInfo.dat");

        SaveData save = new SaveData(score, gameOverNumber, shopItems);

        bf.Serialize(file, save);
        file.Close();
    }
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/saveInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveInfo.dat", FileMode.Open);
            SaveData save = (SaveData)bf.Deserialize(file);
            file.Close();
            save.Loader(this);
        }
    }
    //======================================================================
}
//SAVE FILE
[Serializable]
class SaveData
{
    public float score;
    public int gameOverNumber;
    public ShopItem[] shopItems;
    //place bought powerups here
    public SaveData(float s, int g, ShopItem[] sh)
    {
        score = s;
        gameOverNumber = g;
        shopItems = sh;
    }
    public void Loader(GameManager g)
    {
        g.score = score;
        g.gameOverNumber = gameOverNumber;
        g.shopItems = shopItems;
        foreach(ShopItem s in g.shopItems)
        {
            s.Loaded();
        }
    }
}
