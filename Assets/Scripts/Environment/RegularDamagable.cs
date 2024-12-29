using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularDamagable : MonoBehaviour, ICauseDamage
{
    public int damage; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CauseDamageOnImpact(float velocity, int damage, HealthSystem healthSystem)
    {
        //int roundedVelocity = Mathf.RoundToInt(velocity);
        healthSystem.GetDamaged(damage);
    }

    public void DieUponImpact()
    {
        //  NA whoops
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return; 
        float impactVelocity = collision.relativeVelocity.magnitude;
        if (collision.gameObject.GetComponent<HealthSystem>() && impactVelocity > 20)
        {
            CauseDamageOnImpact(impactVelocity, damage, collision.gameObject.GetComponent<HealthSystem>());
            Debug.Log("Caused Damage Upon Imapact");
        }
    }
}
