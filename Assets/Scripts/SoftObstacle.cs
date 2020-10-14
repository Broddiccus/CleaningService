using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftObstacle : MonoBehaviour
{
    //VARIABLES
    public float slowSpeed = 0.5f;
    //===================================================
    //INHERITED FUNCTIONS
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            HitPlayer();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            EndHit();
        }
    }
    //===================================================
    //VARIABLE FUNCTIONS
    private void HitPlayer()
    {
        LevelHandler.Instance.SetScrollSpeed(slowSpeed);
    }
    private void EndHit()
    {
        LevelHandler.Instance.SetScrollSpeed(LevelHandler.Instance.scrollSpeed);
    }
    //===================================================
}
