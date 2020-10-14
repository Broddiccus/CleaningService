using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    public LevelSet obsRef; //MUST BE UPDATED IN HERE SOMEWHERE WITH THE ONE IN INFINITE SCROLL
    public float spawnDelay;
    public int spawnMin = 1;
    public int spawnMax = 4;
    private void Start()
    {
        ObjectSpawning();
        InfiniteScroll.Instance.onStageChange += StageChange;
        GameManager.Instance.onTransitionStageChange += TransitionStageSet;
    }
    public void ObjectSpawning()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            StopAllCoroutines();
            StartCoroutine("SpawnSection", Random.Range(spawnMin, spawnMax));
        }
    }
    IEnumerator SpawnSection(int n)
    {
        int temp = 0;
        while (temp < n)
        {
            if (!obsRef.trans)
                InfiniteScroll.Instance.SpawnNew(obsRef.Obstacles[Random.Range(0, obsRef.Obstacles.Count)], false);
            if (obsRef.trans)
                InfiniteScroll.Instance.SpawnNew(obsRef.transObstacles[Random.Range(0, obsRef.transObstacles.Count)], false);
            yield return new WaitForSeconds(0.9f);
            temp++;
        }
        Invoke("ObjectSpawning", spawnDelay);
        StopAllCoroutines();
    }
    public void StageChange()
    {
        obsRef = InfiniteScroll.Instance.currentStage;
    }
    public void TransitionStageSet(bool t)
    {
        obsRef.trans = t;
    }
}
