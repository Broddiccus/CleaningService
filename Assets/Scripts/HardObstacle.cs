using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardObstacle : MonoBehaviour
{
    //VARIABLES
    public float hitDuration = 1.0f;
    public float slowSpeed = 0.5f;
    private float damage = -10.0f;
    //===================================================
    //INHERITED FUNCTIONS
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            HitPlayer();
            gameObject.GetComponent<BoxCollider>().enabled = false;
            
        }
    }
    //===================================================
    //VARIABLE FUNCTIONS
    protected virtual void HitPlayer()
    {
        GameManager.Instance.HealthChange(damage);
        LevelHandler.Instance.SetScrollSpeed(slowSpeed);
        StartCoroutine(CameraShake.Instance.Shake(0.2f, 0.1f));
        Invoke("EndHit", hitDuration);
    }
    protected virtual void EndHit()
    {
        LevelHandler.Instance.SetScrollSpeed(LevelHandler.Instance.scrollSpeed);
    }
    public virtual void Reset()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
    //===================================================
}
