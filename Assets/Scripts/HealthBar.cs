using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Health))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _healthSlider = null;

    public Health Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<Health>();

        _healthSlider.maxValue = Health._maxHealth;
        _healthSlider.value = Health._maxHealth;
    }

    public void Update()
    {
        _healthSlider.maxValue = Health._maxHealth;
        _healthSlider.value = Health._currentHealth;
    }

    private void OnEnable()
    {
        Health.Damaged += OnTakeDamage;
        Health.Healed += OnHealed;
    }

    private void OnDisable()
    {
        Health.Damaged -= OnTakeDamage;
        Health.Healed -= OnHealed;
    }

    void OnTakeDamage(int damage)
    {
        _healthSlider.value = Health._currentHealth;
    }

    void OnHealed(int value)
    {
        _healthSlider.value = Health._currentHealth;
    }
}
