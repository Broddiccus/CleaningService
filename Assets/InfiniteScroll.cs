using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//used for scrolling the background and managing spawned obstacles
public class InfiniteScroll : MonoBehaviour
{
    //SINGLETON
    public static InfiniteScroll Instance;
    //OBSTACLE
    public Transform[] obstacleSpawnPosition;
    public List<GameObject> obstacles = new List<GameObject>();
    //BACKGROUND
    public GameObject backgroundPrefab;
    public Transform backgroundSpawnPosition;
    public List<GameObject> backgrounds = new List<GameObject>();
    //SCROLLING
    public float despawnDistance = -10.0f;
    public float scrollSpeed = 1.0f;
    
    

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
    //======================================================================
    //GAMEPLAY CYCLE (SPAWNING AND DESPAWNING)
    private void Update()
    {
        for (int i = 0; i < obstacles.Count; i++)
        {
            if (Mathf.Approximately(DistanceCheck(obstacles[i]), 0.0f))
            {
                Despawn(obstacles[i], false);
                //SpawnNew(obstaclePrefab[0], false);
            }
        }
        for (int i = 0; i < backgrounds.Count; i++)
        {
            if (DistanceCheck(backgrounds[i]) == 0.0f)
            {
                Despawn(backgrounds[i], true);
                SpawnNew(backgroundPrefab, true);
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
                int tempI = Random.Range(0, obstacleSpawnPosition.Length);
                temp = Instantiate(x, obstacleSpawnPosition[tempI].position, obstacleSpawnPosition[tempI].rotation, transform);
                obstacles.Add(temp);
                break;
        }
    }

    private void Despawn(GameObject x, bool background)
    {
        switch (background)
        {
            case true:
                backgrounds.Remove(x);
                Destroy(x);
                break;
            case false:
                obstacles.Remove(x);
                Destroy(x);
                break;
        }
    }
    //======================================================================
    //PRACTICAL FUNCTIONS
    private float DistanceCheck(GameObject x)
    {
        Vector3 temp = new Vector3(despawnDistance, x.transform.position.y, x.transform.position.z);
        x.transform.position = Vector3.MoveTowards(x.transform.position, temp, scrollSpeed * Time.deltaTime);
        return Vector3.Distance(x.transform.position, temp);
    }
    //======================================================================
}
