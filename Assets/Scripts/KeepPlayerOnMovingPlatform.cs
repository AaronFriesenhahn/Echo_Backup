using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepPlayerOnMovingPlatform : MonoBehaviour
{
    private GameObject target = null;
    private float offset;
    void Start()
    {
        target = null;
    }
    void OnTriggerStay(Collider col)
    {
        target = col.gameObject;
        target.transform.position = Vector3.MoveTowards(target.transform.position, transform.position, 0);
    }
    void OnTriggerExit(Collider col)
    {
        target = null;
    }
}
