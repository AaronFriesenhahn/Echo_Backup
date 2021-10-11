using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundDetector : MonoBehaviour
{
    public event Action GroundDetected = delegate { };
    public event Action GroundVanished = delegate { };

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Ground")
        {
            GroundDetected?.Invoke();
            Debug.Log("Detecting Ground.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GroundVanished?.Invoke();
        //Debug.Log("Ground Left.");
    }
}
