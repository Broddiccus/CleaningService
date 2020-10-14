using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCrash : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(CameraShake.Instance.Shake(0.5f, 0.3f));
    }
}
