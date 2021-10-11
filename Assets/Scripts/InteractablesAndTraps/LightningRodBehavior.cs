using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRodBehavior : MonoBehaviour
{
    public event Action Powered = delegate { };

    public bool HitByElectric = false;
    public bool ObjectPowered = false;
    [SerializeField] GameObject[] _ObjectsToPower;
    [SerializeField] ParticleSystem _PoweredEffect;
    public int TimePowered = 5;

    // Update is called once per frame
    void Update()
    {
        if (HitByElectric == true)
        {
            //power object
            ObjectPowered = true;
        }
        else if (HitByElectric == false)
        {
            //remove power from object
            ObjectPowered = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerProjectileElectric(Clone)")
        {
            HitByElectric = true;
            StartCoroutine(HitByElectricFeedback());
        }
    }

    IEnumerator HitByElectricFeedback()
    {
        //Spawn Shocked/Powered Feedback Image at objects transform
        ParticleSystem _shockedEffectParticles = Instantiate(_PoweredEffect, gameObject.transform.position, Quaternion.identity);
        //power objects
        for (int i = 0; i < _ObjectsToPower.Length; i++)
        {
            GameObject _objectPowered = _ObjectsToPower[i];
            //_objectPowered.SetActive(false);
            //allow objects to move?
            //check power state and toggle
            if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == false)
            {
                _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = true;
            }
            else if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == true)
            {
                _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = false;
            }
        }
        //Amount of time object is shocked/powered
        yield return new WaitForSeconds(TimePowered);
        //Destroy Feedback Image, indicating object is no longer shocked/powered
        Destroy(_shockedEffectParticles);
        HitByElectric = false;
        //turn off objects
        for (int i = 0; i < _ObjectsToPower.Length; i++)
        {
            GameObject _objectPowered = _ObjectsToPower[i];
            //_objectPowered.SetActive(false);
            //return object to previous state
            if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == false)
            {
                _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = true;
            }
            else if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == true)
            {
                _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = false;
            }
        }
        Debug.Log("Electric feedback is over.");
    }
}
