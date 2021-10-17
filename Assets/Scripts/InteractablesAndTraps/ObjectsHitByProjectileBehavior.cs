using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsHitByProjectileBehavior : MonoBehaviour
{
    public bool HitByIce = false;
    public bool HitByElectric = false;

    public bool ObjectTypeEnemy = false;
    public bool ObjectTypeCrusher = false;
    public bool ObjectTypeMovingPlatform = false;
    public bool ObjectTypeObjectToPower = false;
    public bool ObjectPowered = false;

    public LightningRodBehavior LightningRod { get; private set; }

    public float ObjectSpeed = 2f;
    public float CrusherSpeed = 2f;
    [SerializeField] GameObject _MovingObject;
    [SerializeField] Transform _StartingPosition;
    [SerializeField] Transform _Destination1;
    //more destinations?
    bool reachDestination1 = false;
    bool reachDestination2 = false;

    public EnemyAI _enemyAi;
    [SerializeField] ParticleSystem _frozenEffect;
    [SerializeField] ParticleSystem _PoweredEffect;

    //testing shock/power effect on enemy
    int i = 0;

    // Update is called once per frame
    void Update()
    {
        if (ObjectTypeObjectToPower == false)
        {
            if (HitByIce == true)
            {
                if (ObjectTypeEnemy == true)
                {
                    _enemyAi.allowfire = false;
                }
            }
            else if (HitByIce == false)
            {
                if (ObjectTypeCrusher == true)
                {
                    CrusherMovement();
                }
                else if (ObjectTypeMovingPlatform == true)
                {
                    MovingPlatformMovement();
                }
            }
        }
        else if (ObjectTypeObjectToPower == true)
        {
            if (ObjectPowered == true)
            {
                if (ObjectTypeCrusher == true)
                {
                    CrusherMovement();
                }
                else if (ObjectTypeMovingPlatform == true)
                {
                    MovingPlatformMovement();
                }
                else if (ObjectTypeEnemy == true)
                {
                    //gameObject.SetActive(false);
                    //adding an affect on enemies(probably will not use, unless I want some sort of chain effect)
                    if (i == 0)
                    {
                        Debug.Log("Shocked and Jumping.");
                        gameObject.GetComponent<Rigidbody>().AddForce(transform.up * 250);
                        i++;
                    }
                }
            }
        }
        //if (HitByElectric == true)
        //{
        //    if (ObjectTypeLightningRod == true)
        //    {
        //        //power object
        //        ObjectPowered = true;
        //    }
        //}
        //if (HitByElectric == false)
        //{
        //    //remove power from object
        //    ObjectPowered = false;
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerProjectileIce(Clone)")
        {
            HitByIce = true;
            StartCoroutine(HitByIceFeedback());
        }
        if (collision.gameObject.name == "PlayerProjectileElectric(Clone)")
        {
            HitByElectric = true;

        }
    }

    IEnumerator HitByIceFeedback()
    {
        //spawn Frozen Feedback Image at objects transform
        ParticleSystem _frozenEffectParticles = Instantiate(_frozenEffect, gameObject.transform.position, Quaternion.identity);
        //Have time for object frozen?
        yield return new WaitForSeconds(3f);
        //destroy Frozen Feedback Image, indicating object is no longer frozen
        Destroy(_frozenEffectParticles);

        HitByIce = false;
        if (ObjectTypeEnemy == true)
        {
            _enemyAi.allowfire = true;
        }        
    }

    IEnumerator HitByElectricFeedback()
    {
        //Spawn Shocked/Powered Feedback Image at objects transform
        ParticleSystem _shockedEffectParticles = Instantiate(_PoweredEffect, gameObject.transform.position, Quaternion.identity);
        //Amount of time object is shocked/powered
        yield return new WaitForSeconds(3f);
        //Destroy Feedback Image, indicating object is no longer shocked/powered
        Destroy(_shockedEffectParticles);
        HitByElectric = false;
    }

    //Moves Crusher Between 2 points (essentially StartingPosition - Destination1) 
    //(GameObjects do not represent actual points where Crusher will move to, just how much it will move)
    //If StartingPosition.y is 0 and Destination1.y is -4, the Crusher will move up and down by 3
    public void CrusherMovement()
    {
        //move to StartingPosition
        if (_MovingObject.transform.position != _StartingPosition.position && reachDestination1 == false)
        {
            float step = ObjectSpeed * Time.deltaTime;
            _MovingObject.transform.position = Vector3.MoveTowards(_MovingObject.transform.position, _StartingPosition.position, step);
        }
        //move to Destination1
        else if (_MovingObject.transform.position != _Destination1.position && reachDestination2 == false)
        {
            float step = (ObjectSpeed * CrusherSpeed) * Time.deltaTime;
            transform.position = Vector3.MoveTowards(_MovingObject.transform.position, _Destination1.position, step);
        }
        //if reached StartingPosition, swap to Destination1
        if (Vector3.Distance(_MovingObject.transform.position, _StartingPosition.position) < 1f)
        {
            reachDestination1 = true;
            reachDestination2 = false;
            //Debug.Log("reached destination 1.");
        }
        //if reached Destination1, swap to StartingPosition
        else if (Vector3.Distance(_MovingObject.transform.position, _Destination1.position) < 1f)
        {
            reachDestination2 = true;
            reachDestination1 = false;
            //Debug.Log("reached destination 2.");
        }
    }
    //Moves Platform Between points
    //(GameObjects do not represent actual points where Platform will move to, just how much it will move)
    //If StartingPosition.y is 0 and Destination1.y is -4, the Crusher will move up and down by 3
    public void MovingPlatformMovement()
    {
        //move to StartingPosition
        if (_MovingObject.transform.position != _StartingPosition.position && reachDestination1 == false)
        {
            float step = ObjectSpeed * Time.deltaTime;
            _MovingObject.transform.position = Vector3.MoveTowards(_MovingObject.transform.position, _StartingPosition.position, step);
        }
        //move to Destination1
        else if (_MovingObject.transform.position != _Destination1.position && reachDestination2 == false)
        {
            float step = ObjectSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(_MovingObject.transform.position, _Destination1.position, step);
        }
        //if reached StartingPosition, swap to Destination1
        if (Vector3.Distance(_MovingObject.transform.position, _StartingPosition.position) < 1f)
        {
            reachDestination1 = true;
            reachDestination2 = false;
            //Debug.Log("reached destination 1.");
        }
        //if reached Destination1, swap to StartingPosition
        else if (Vector3.Distance(_MovingObject.transform.position, _Destination1.position) < 1f)
        {
            reachDestination2 = true;
            reachDestination1 = false;
            //Debug.Log("reached destination 2.");
        }
    }

}
