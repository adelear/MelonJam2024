using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnHealthMaxChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    private int healthMax;
    private int health;

    public HealthSystem(int healthMax)
    {
        this.healthMax = healthMax; 
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
}
