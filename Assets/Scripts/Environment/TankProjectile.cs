using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProjectile : MonoBehaviour, ICauseDamage
{
    int damage = 10; 
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
        Destroy(gameObject, 2f); 
    }
} 
