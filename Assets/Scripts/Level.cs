using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameManager.StageSet stage;
    public List<GameObject> LevelTiles;
    public List<GameObject> TransLevelTiles;
    public List<GameObject> StartingTiles;
    public List<GameObject> TransitionTiles;
}
