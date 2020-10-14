using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionBox : MonoBehaviour
{
    private void Awake()
    {
        transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
    }
}
