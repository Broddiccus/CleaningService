using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//used for scrolling the background and managing spawned obstacles
public class InfiniteScroll : MonoBehaviour
{
    //SINGLETON
    public static InfiniteScroll Instance;
    //STAGESETS
    public LevelSet currentStage;
    public LevelSet[] Stages; //contains references to all the various object types for each stage
    public event Action onStageChange;
    //OBSTACLE
    public Transform[] obstacleSpawnPosition;
    public List<GameObject> obstacles = new List<GameObject>(); //contains the current list of obstacles on screen
    //BACKGROUND
    public Transform backgroundSpawnPosition;
    public List<GameObject> backgrounds = new List<GameObject>(); //contains the current list of backgrounds on screen
    //DUST
    public Transform[] dustSpawnPosition;
    public GameObject dustPrefab;
    public List<GameObject> dustList = new List<GameObject>();
    //SCROLLING
    public enum OBJType { Background, Obstacle, Dust };
    public float despawnDistance = -10.0f;
    public float scrollSpeed = 1.0f;
    private float scrollingSpeed;
    
    

    //SINGLETON INITIALIZATION
    //======================================================================
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
        
        scrollingSpeed = scrollSpeed;
        GameManager.Instance.onGameRestart += Reset;
        GameManager.Instance.onTransitionStageChange += TransitionStageSet;
        DustSpawner(dustPrefab);
    }
    //======================================================================
    //STAGE SET CHANGING
    public void StageChange(GameManager.StageSet s) //NEED TO CALL THIS FUNCTION FROM GAMEMANAGER ON LEVEL SWITCH AND ALSO ON STARTUP
    {
        currentStage = Stages[(int)s];
        if (onStageChange != null)
            onStageChange();
    }
    public void TransitionStageSet(bool t)
    {
        currentStage.trans = t;
    }
    //======================================================================
    //GETTER/SETTER
    public void SetScrollSpeed(float speed)
    {
        scrollingSpeed = speed;
    }
    public float GetSpeed()
    {
        return scrollingSpeed;
    }
    //======================================================================
    //GAMEPLAY CYCLE (SPAWNING AND DESPAWNING)
    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (Mathf.Approximately(DistanceCheck(obstacles[i]), 0.0f))
                {
                    Despawn(obstacles[i], OBJType.Obstacle);
                    //SpawnNew(obstaclePrefab[0], false);
                }
            }
            for (int i = 0; i < backgrounds.Count; i++)
            {
                if (DistanceCheck(backgrounds[i]) == 0.0f)
                {
                    Despawn(backgrounds[i], OBJType.Background);
                    if (!currentStage.trans)
                        SpawnNew(currentStage.Backgrounds[UnityEngine.Random.Range(0,currentStage.Backgrounds.Count)], true);
                    if (currentStage.trans)
                        SpawnNew(currentStage.transBackgrounds[UnityEngine.Random.Range(0, currentStage.transBackgrounds.Count)], true);
                }
            }
            for (int i = 0; i < dustList.Count; i++)
            {
                float temp = DistanceCheck(dustList[i]);
                if (i == dustList.Count-1)
                {
                    Dust dustScript = dustList[i].GetComponent<Dust>();
                    if (temp < 48 - dustScript.width)
                    {
                        DustSpawner(dustPrefab);
                    }

                }
                if (Mathf.Approximately(temp, 0.0f))
                {
                    Despawn(dustList[i], OBJType.Dust);
                    //SpawnNew(obstaclePrefab[0], false);
                }
            }
        }
    }

    public void SpawnNew(GameObject x, bool background)
    {
        GameObject temp;
        switch (background)
        {
            case true:
                temp = Instantiate(x, backgroundSpawnPosition.position, backgroundSpawnPosition.rotation, transform);
                backgrounds.Add(temp);
                break;
            case false:
                int tempI = UnityEngine.Random.Range(0, obstacleSpawnPosition.Length);
                temp = Instantiate(x, obstacleSpawnPosition[tempI].position, obstacleSpawnPosition[tempI].rotation, transform);
                obstacles.Add(temp);
                break;
        }
    }

    public void Despawn(GameObject x, OBJType t)
    {
        switch (t)
        {
            case OBJType.Background:
                backgrounds.Remove(x);
                Destroy(x);
                break;
            case OBJType.Obstacle:
                obstacles.Remove(x);
                Destroy(x);
                break;
            case OBJType.Dust:
                dustList.Remove(x);
                Destroy(x);
                break;
        }
    }
    //======================================================================
    //PRACTICAL FUNCTIONS
    private float DistanceCheck(GameObject x)
    {
        Vector3 temp = new Vector3(despawnDistance, x.transform.position.y, x.transform.position.z);
        x.transform.position = Vector3.MoveTowards(x.transform.position, temp, scrollingSpeed * Time.deltaTime);
        return Vector3.Distance(x.transform.position, temp);
    }
    private void DustSpawner(GameObject x)
    {
        GameObject temp;
        int tempI = UnityEngine.Random.Range(0, dustSpawnPosition.Length);
        temp = Instantiate(x, dustSpawnPosition[tempI].position, dustSpawnPosition[tempI].rotation, transform);
        dustList.Add(temp);
    }
    public void Reset()
    {
        for (int i = 0; i < obstacles.Count; i++)
            Despawn(obstacles[i], OBJType.Obstacle);
        for (int i = 0; i < dustList.Count; i++)
            Despawn(dustList[i], OBJType.Dust);
        DustSpawner(dustPrefab);
    }
    //======================================================================
}
