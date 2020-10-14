using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//dust will have 5 possible spawn positions
//it must have reference  the Dust behind it
//as well as reference to each sprite of dust
//an on trigger enter with player to increase score and multi
public class Dust : MonoBehaviour
{
    public int position;
    public float scoregain;
    public Dust previous;
    public float width;
    public GameObject face;
    private void Awake()
    {
        width = gameObject.GetComponent<BoxCollider>().size.x;
    }
    public Dust(Dust d, int p, Transform pos)
    {
        previous = d;
        position = p;
        transform.position = pos.position;
        width = gameObject.GetComponent<BoxCollider>().bounds.size.x;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && GameManager.Instance.gameState == GameManager.GameState.Gameplay)
        {
            HitPlayer();

        }
    }
    private void HitPlayer()
    {
        int temp = Mathf.RoundToInt(GameManager.Instance.ScoreGain(scoregain));
        if (temp == 0)
            temp = 1;
        for (int i = 0; i < temp; i++)
            Instantiate(GameManager.Instance.CoinPrefab, transform.position, transform.rotation);
        face.SetActive(false);
    }
    public void DustReset()
    {
        face.SetActive(true);
    }
}
