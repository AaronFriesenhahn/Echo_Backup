using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    public ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;
    public Transform _PlayerTransform;
    public bool _isFacingLeft = true;
    public EnemyRespawner _enemyRespawner;

    public AudioClip _deathSound;

    Rigidbody _rb;
    Health _healthsystem;
    //public int _MaxHealth = 1;
    //public int _currentHealth;

    [SerializeField] GameObject PowerupType;

    float speed = 2f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _healthsystem = GetComponent<Health>();
    }

    private void ImpactFeedback()
    {
        //particles
        if (_impactParticles != null)
        {
            _impactParticles = Instantiate(_impactParticles,
                transform.position, Quaternion.identity);
        }
        //audio. TODO - consider Object Pooling for performance
        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, 1f);
        }
    }

    private void FixedUpdate()
    {
        GameObject PlayerFound = GameObject.FindGameObjectWithTag("Player");
        if (PlayerFound != null)
        {
            Move();
        }
        if (_healthsystem._currentHealth <= 0)
        {
            DeathFeedback();
        }
        RotateToFacePlayer();
    }

    protected virtual void Move()
    {
        //For enemies to run towards the player
        //var targetPosition = new Vector3(_PlayerTransform.position.x, _PlayerTransform.position.y, _PlayerTransform.position.z);
        //// Move our position a step closer to the target.
        //float step = speed * Time.deltaTime; // calculate distance to move
        //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }
    //rotates enemy to face player
    protected virtual void RotateToFacePlayer()
    {
        if (_PlayerTransform.transform.position.x < transform.position.x && _isFacingLeft == false)
        {
            //Face Left
            transform.Rotate(0, 180, 0);
            _isFacingLeft = true;
        }
        else if (_PlayerTransform.transform.position.x > transform.position.x && _isFacingLeft == true)
        {
            //Face Right
            transform.Rotate(0, -180, 0);
            _isFacingLeft = false;
        }
    }

    protected virtual void DeathFeedback()
    {
        ParticleSystem _particles = Instantiate(_impactParticles,
                transform.position, Quaternion.identity);
        AudioHelper.PlayClip2D(_deathSound, 1f);

        //upon death spawn a powerup based on enemy type
        GameObject powerup = Instantiate(PowerupType, transform.position, transform.rotation);

        //TODO Respawn functionality 
        Debug.Log("Calling Respawn");
        if (_enemyRespawner == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _enemyRespawner.StartCoroutine(_enemyRespawner.RespawnDelay());
        }        
    }

    //testing extra damage from stronger type. It works!!! Add element type between Projectile and (Clone) Perhaps add to EnemyType Scripts
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PlayerProjectile(Clone)")
        {
            Debug.Log("Hit by " + collision.gameObject.name + ". Adding extra damage.");
            _healthsystem.TakeDamage(5);
        }

    }
}