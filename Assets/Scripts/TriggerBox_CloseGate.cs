using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBox_CloseGate : MonoBehaviour
{
    [SerializeField] GameObject[] _ObjectsToPower;
    [SerializeField] ETypeBoss _boss;
    public bool TogglesPower = true;
    public float TimePowered = 5;
    public bool TriggerOnce = true;
    int x = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player has entered Room Before Boss.");
            if (x == 1)
            {
                StartCoroutine(HitByElectricFeedback());
                _boss._PlayerHasEntered = true;
                _boss.allowfire = true;
                _boss.GetComponent<Health>().invincible = false;
            }
            if (TriggerOnce == true)
            {
                x = 0;
            }
        }
    }

    IEnumerator HitByElectricFeedback()
    {
        //Spawn Shocked/Powered Feedback Image at objects transform
        //power objects
        for (int i = 0; i < _ObjectsToPower.Length; i++)
        {
            GameObject _objectPowered = _ObjectsToPower[i];
            //_objectPowered.SetActive(false);
            //allow objects to move?
            //check power state and toggle
            //checks for hit by projectile behavior. If there is one, check if object is powered.
            if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>() != null)
            {
                if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == false)
                {
                    _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = true;
                }
                else if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == true)
                {
                    _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = false;
                }
            }
            //checks for turret behavior. If there is one, check if object is powered.
            else if (_objectPowered.GetComponent<TurretBehavior>() != null)
            {
                if (_objectPowered.GetComponent<TurretBehavior>().ObjectPowered == false)
                {
                    _objectPowered.GetComponent<TurretBehavior>().ObjectPowered = true;
                }
                else if (_objectPowered.GetComponent<TurretBehavior>().ObjectPowered == true)
                {
                    _objectPowered.GetComponent<TurretBehavior>().ObjectPowered = false;
                }
            }
        }
        //Amount of time object is shocked/powered
        yield return new WaitForSeconds(TimePowered);
        //Destroy Feedback Image, indicating object is no longer shocked/powered
        //returns objects to normal
        if (TogglesPower == false)
        {
            for (int i = 0; i < _ObjectsToPower.Length; i++)
            {
                GameObject _objectPowered = _ObjectsToPower[i];
                //check power state and toggle
                //checks for hit by projectile behavior. If there is one, check if object is powered.
                if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>() != null)
                {
                    if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == false)
                    {
                        _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = true;
                    }
                    else if (_objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered == true)
                    {
                        _objectPowered.GetComponent<ObjectsHitByProjectileBehavior>().ObjectPowered = false;
                    }
                }
                //checks for turret behavior. If there is one, check if object is powered.
                else if (_objectPowered.GetComponent<TurretBehavior>() != null)
                {
                    if (_objectPowered.GetComponent<TurretBehavior>().ObjectPowered == false)
                    {
                        _objectPowered.GetComponent<TurretBehavior>().ObjectPowered = true;
                    }
                    else if (_objectPowered.GetComponent<TurretBehavior>().ObjectPowered == true)
                    {
                        _objectPowered.GetComponent<TurretBehavior>().ObjectPowered = false;
                    }
                }
            }
        }
    }
}
