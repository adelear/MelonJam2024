using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectile : MonoBehaviour, ICauseDamage
{
    int damage = 5; 
    public void CauseDamageOnImpact(float velocity, int damage, HealthSystem healthSystem)
    {
        healthSystem.GetDamaged(damage);
    }

    public void DieUponImpact()
    {
        Destroy(gameObject); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystem hs = collision.gameObject.GetComponent<HealthSystem>();
            CauseDamageOnImpact(0f, damage, hs); 
            DieUponImpact(); 
        }
        if (collision.gameObject.GetComponent<Animal>())
        {
            Animal animal = collision.gameObject.GetComponent<Animal>();
            animal.DieUponImpact(); 
        }
        Destroy(gameObject, 2f); 
    }
} 
