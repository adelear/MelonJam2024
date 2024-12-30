using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblesOnVelocity : MonoBehaviour
{
    [SerializeField] GameObject crumbledObject;
    public float threshold;
    public int health = 100;
    [SerializeField] private AudioClip destroyedSound;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            //Debug.Log("Velocity: " + collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude > threshold)
            {
                health -= 10; 
                Debug.Log("Health: " + health);
                if (health <= 0) Crumble();
            }
        }
    }

    private void Crumble()
    {
        crumbledObject.SetActive(true);
        AudioManager.Instance.PlayOneShotWithRandomPitch(destroyedSound, false, 0.8f, 1.2f, 0.4f); 
        gameObject.SetActive(false);
        Debug.Log("Object crumbled!");
        Destroy(crumbledObject,10f);
        Destroy(gameObject,10f); 
    }
}
