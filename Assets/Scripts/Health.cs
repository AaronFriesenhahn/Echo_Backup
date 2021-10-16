using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    public int _maxHealth = 3;
    public int _currentHealth;
    public bool KillObject = false;
    string ObjectName;
    [SerializeField]
    ParticleSystem _impactParticles;

    GameObject _player;

    public event Action<int> Damaged = delegate { };
    public event Action<int> Healed = delegate { };

    [SerializeField] ParticleSystem _hitParticles;

    public bool invincible = false;

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;
        ObjectName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage taken: " + damage);
        _currentHealth -= damage;
        Damaged.Invoke(damage);
        _impactParticles = Instantiate(_impactParticles, transform.position, Quaternion.identity);
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            if (invincible == false)
            {
                Kill();
            }
        }
    }

    public void IncreaseHealth(int value)
    {
        _currentHealth += value;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Debug.Log("Increase player health.");
        Healed.Invoke(value);
    }

    private void Kill()
    {
        KillObject = true;
        var objectMesh = gameObject.GetComponent<MeshRenderer>();
        var objectCollider = gameObject.GetComponent<Collider>();

        //objectMesh.enabled = false;
        //objectCollider.enabled = false;
        //gameObject.SetActive(false);
    }

    private void HitFlash()
    {
        Debug.Log("Hit. Applying Visual Feedback.");
        //_hitParticles = Instantiate(_hitParticles, transform.position, Quaternion.identity);
    }
}
