using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectPartsBase : MonoBehaviour
{
    protected abstract void Collect(PlayerController player);

    [SerializeField] float _movementSpeed = 1;
    protected float MovementSpeed => _movementSpeed;

    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSound;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        //calculate rotation
        Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            Collect(player);
            //get mesh of Collectable
            var powerupMesh = gameObject.GetComponent<MeshRenderer>();
            var powerupCollider = gameObject.GetComponent<Collider>();
            //spawn particles & sfx because we need to disable object
            Feedback();

            //gameObject.SetActive(false);
            powerupMesh.enabled = false;
            powerupCollider.enabled = false;
            
        }
    }

    protected virtual void Feedback()
    {
        //particles
        if (_collectParticles != null)
        {
            _collectParticles = Instantiate(_collectParticles,
                transform.position, Quaternion.identity);
        }
        //audio
        if (_collectSound != null)
        {
            AudioHelper.PlayClip2D(_collectSound, 1f);
        }
    }
}
