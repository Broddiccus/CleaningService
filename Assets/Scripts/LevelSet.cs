using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelSet : MonoBehaviour
{
    //VARIABLES
    public GameManager.StageSet stage;
    public List<GameObject> Obstacles;
    public List<GameObject> Backgrounds;
    public List<GameObject> transObstacles;
    public List<GameObject> transBackgrounds;
    public bool trans = false;
    public GameObject dust;

}
