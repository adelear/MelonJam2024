using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("UI Settings")]
    public Image healthBarImage;

    void Start()
    {
        health = healthMax;
        UpdateHealthBar(); 
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
        UpdateHealthBar();

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
        UpdateHealthBar();
    }

    public void Die()
    {
        Debug.Log("Character has died.");
        OnDead?.Invoke(this, EventArgs.Empty);

        if (gameObject.CompareTag("Player"))
        {
            GameManager.Instance.SwitchState(GameManager.GameState.DEFEAT); 
        }
        else
        {
            Destroy(gameObject);
        }
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
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            float currentHeight = healthBarImage.rectTransform.sizeDelta.y;
            float healthRatio = GetHealthToHealthMaxRatio();
            float targetHeight = healthRatio * healthBarImage.rectTransform.sizeDelta.y;
            float newHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * 10f);

            healthBarImage.rectTransform.sizeDelta = new Vector2(healthBarImage.rectTransform.sizeDelta.x, newHeight);
        }
    }

}
