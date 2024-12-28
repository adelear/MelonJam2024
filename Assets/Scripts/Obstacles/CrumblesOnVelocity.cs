using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblesOnVelocity : MonoBehaviour
{
    [SerializeField] GameObject crumbledObject;
    public float threshold; 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            Debug.Log("Velocity: " + collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude); 
            if (collision.gameObject.GetComponent<Rigidbody>().velocity.magnitude > threshold)
            {
                Instantiate(crumbledObject, gameObject.transform.position, gameObject.transform.rotation);
                Destroy(gameObject); 
                // Just have the building with the destroyed version of itself in the same spot, just set inactive,
                // and instead of destroying and spawning, set this object inactive, set destroyed object active
            } 
        }
    }
}
