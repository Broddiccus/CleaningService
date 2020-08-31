using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleHandler : MonoBehaviour
{
    public GameObject[] obstaclePrefab;
    public float spawnDelay;
    public bool spawn = true;
    private void Start()
    {
        StartCoroutine("ObjectSpawning");
    }
    IEnumerator ObjectSpawning()
    {
        while (spawn)
        {
            InfiniteScroll.Instance.SpawnNew(obstaclePrefab[Random.Range(0, obstaclePrefab.Length)], false);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
