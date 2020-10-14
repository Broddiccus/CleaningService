using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Coin : MonoBehaviour
{
    Vector3 endPoint;
    private void Start()
    {
        endPoint = Camera.main.ScreenToWorldPoint(UIManager.Instance.Score.transform.position);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPoint, 10.0f * Time.deltaTime);
        if (Mathf.Approximately(Vector3.Distance(transform.position, endPoint),0.0f))
            Destroy(gameObject);
    }
}
