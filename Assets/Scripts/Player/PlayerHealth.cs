using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public delegate void OnPlayerDeath();
    public static event OnPlayerDeath _onPlayerDeath;

    public delegate void OnHealthChange(float currentHealth);
    public static event OnHealthChange _onHealthChange;

    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        _onHealthChange?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            _onPlayerDeath?.Invoke();
        }

    }
}
