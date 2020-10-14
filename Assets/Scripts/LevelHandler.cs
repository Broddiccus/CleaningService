using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    //just stop them moving at the end, and then randomly pick from the stuff off screen to send to the front
    //then swap the prefabs out on a Game Manager Function when new level sets are selected
    //SINGLETON
    public static LevelHandler Instance;
    //LEVELREFERENCE
    public List<Level> levelRef;
    [HideInInspector]
    public List<Level> currentLevel;
    [HideInInspector]
    public List<GameObject> currentOBJ = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> currentTransOBJ = new List<GameObject>(); //element 0 in this is for transition to transition stage
    public List<GameObject> activeOBJ = new List<GameObject>();
    public List<GameObject> transOBJ = new List<GameObject>();
    //SPEED
    public float baseScrollSpeed = 1.0f;
    public float scrollSpeed;
    public float speedInc = 10.0f;
    public float speedIncval = 0.0f;
    [HideInInspector]
    public float scrollingSpeed;
    public float stopDistance = -16.0f;
    //SPAWNING
    public Transform spawnLoc;
    public Vector3[] StartTilePositions;
    public bool transitioning = false;

    //======================================================================
    //INSTANTIATION
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
        LevelSetup(GameManager.Instance.currentStage);
    }
    public void GameStart()
    {
        InvokeRepeating("SpeedUp", speedInc, speedInc);
    }
    public void GameEnd()
    {
        CancelInvoke();
        scrollSpeed = baseScrollSpeed;
    }
    public void LevelSetup(GameManager.StageSet s)
    {
        CancelInvoke();
        
        scrollSpeed = baseScrollSpeed;
        scrollingSpeed = scrollSpeed;
        foreach(Level x in levelRef)
        {
            if (x.stage == s)
            {
                foreach (GameObject y in activeOBJ)
                {
                    Destroy(y);
                }
                foreach (GameObject y in currentOBJ)
                {
                    Destroy(y);
                }
                foreach (GameObject y in currentTransOBJ)
                {
                    Destroy(y);
                }
                activeOBJ.Clear();
                currentOBJ.Clear();
                currentTransOBJ.Clear();
                foreach (GameObject y in x.StartingTiles)
                {
                    GameObject temp = Instantiate(y, transform);
                    activeOBJ.Add(temp);
                }
                foreach (GameObject y in x.LevelTiles)
                {
                    GameObject temp = Instantiate(y, transform);
                    currentOBJ.Add(temp);
                }
                foreach (GameObject y in x.TransLevelTiles)
                {
                    GameObject temp = Instantiate(y, transform);
                    currentTransOBJ.Add(temp);
                }
                foreach (GameObject y in x.TransitionTiles)
                {
                    GameObject temp = Instantiate(y, transform);
                    transOBJ.Add(temp);
                }
                
                for (int i = 0; i < activeOBJ.Count; i++) //places opening tiles
                {
                    activeOBJ[i].transform.position = StartTilePositions[i];
                }
                return;
            }
        }
        Debug.Log("NO LEVEL");
    }
    
    //======================================================================
    //GAMEPLAY
    private void Update()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            
            for (int i = 0; i < activeOBJ.Count; i++)
            {
                if (DistanceCheck(activeOBJ[i]) == 0.0f)
                {
                    BlockChange(activeOBJ[i]);
                }
            }
        }
    }

    private void BlockChange(GameObject x)
    {
        activeOBJ.Remove(x);
        if (!x.GetComponent<SegmentHandler>().transitioningPiece)
        {
            if (x.GetComponent<SegmentHandler>().transition)
            {
                currentTransOBJ.Add(x);
            }
            else
            {
                currentOBJ.Add(x);
            }
        }
        int temp = 0;
        bool checker = false;
        switch (GameManager.Instance.transitionStage)
        {
            case true:
                if (!transitioning)
                {
                    while (!checker)
                    {
                        temp = Random.Range(0, currentTransOBJ.Count);
                        if (currentTransOBJ[temp].GetComponent<SegmentHandler>().difficulty != activeOBJ[activeOBJ.Count - 1].GetComponent<SegmentHandler>().difficulty)
                            checker = true;
                    }
                    activeOBJ.Add(currentTransOBJ[temp]);
                    currentTransOBJ.Remove(currentTransOBJ[temp]);
                }
                else
                {
                    activeOBJ.Add(transOBJ[0]);
                    transitioning = false;
                }
                
                break;
            case false:
                if (!transitioning)
                {
                    while (!checker)
                    {
                        temp = Random.Range(0, currentOBJ.Count);
                        if (currentOBJ[temp].GetComponent<SegmentHandler>().difficulty != activeOBJ[activeOBJ.Count - 1].GetComponent<SegmentHandler>().difficulty)
                            checker = true;
                    }
                    activeOBJ.Add(currentOBJ[temp]);
                    currentOBJ.Remove(currentOBJ[temp]);
                }
                else
                {
                    activeOBJ.Add(transOBJ[1]);
                    transitioning = false;
                }
                break;
        }
        Vector3 tempV = new Vector3(activeOBJ[activeOBJ.Count - 2].transform.position.x + 16, activeOBJ[activeOBJ.Count - 1].transform.position.y, activeOBJ[activeOBJ.Count - 1].transform.position.z);
        activeOBJ[activeOBJ.Count - 1].transform.position = tempV;
        if (activeOBJ[activeOBJ.Count - 1].GetComponent<SegmentHandler>() != null)
            activeOBJ[activeOBJ.Count - 1].GetComponent<SegmentHandler>().Refresh();
    }
    public void SetScrollSpeed(float s)
    {
        scrollingSpeed = s;
    }
    public void IncreaseSpeed(float s)
    {
        scrollSpeed += s;
        SetScrollSpeed(scrollSpeed);
    }
    public void SpeedUp()
    {
        IncreaseSpeed(0.1f - (0.1f * speedIncval));
    }
    //======================================================================
    //TECHNICAL
    private float DistanceCheck(GameObject x)
    {
        Vector3 temp = new Vector3(stopDistance, x.transform.position.y, x.transform.position.z);
        x.transform.position = Vector3.MoveTowards(x.transform.position, temp, scrollingSpeed * Time.deltaTime);
        return Vector3.Distance(x.transform.position, temp);
    }
    //======================================================================
}
