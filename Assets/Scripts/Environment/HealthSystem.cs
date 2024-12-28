using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    [Header("Health Settings")]
    public int healthMax = 100; 
    private int health;

    void Start()
    {
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetHealthMax()
    {
        return healthMax;
    }

    public float GetHealthToHealthMaxRatio()
    {
        return (float)health / healthMax;
    }

    public void GetDamaged(int damage)
    {
        Debug.Log(gameObject.name + " Got Damaged"); 
        health -= damage;
        health = Mathf.Max(health, 0);

        OnDamaged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        if (health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        health = Mathf.Min(health, healthMax);
        OnHealed?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Die()
    {
        Debug.Log("Character has died.");
        OnDead?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject); 
    }

    public void SetMaxHealth(int newHealthMax)
    {
        healthMax = newHealthMax;

        if (health > healthMax)
        {
            health = healthMax;
        }

        OnHealthMaxChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
}
