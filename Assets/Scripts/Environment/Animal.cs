using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, ICauseDamage
{
    public event Action<AnimalStates> onStateChanged;

    [Header("Animal Settings")]
    public AnimalType animalType;

    [Header("Movement")]
    public Transform pointA;
    public Transform pointB;
    public float walkSpeed = 2f;
    public float deathVelocityThreshold = 10f;
    private int damage;

    private AnimalStates currentState;
    private Transform currentTarget;
    private Animator animator;
    private Rigidbody rb; 

    public AnimalStates CurrentState
    {
        get { return currentState; }
        set
        {
            if (currentState != value)
            {
                currentState = value;
                onStateChanged?.Invoke(currentState);
                Debug.Log("State Changed to " + currentState);

                switch (currentState)
                {
                    case AnimalStates.Walking:
                        SetWalkingState();
                        break;
                    case AnimalStates.Idle:
                        SetIdleState();
                        break;
                    case AnimalStates.Falling:
                        SetFallingState();
                        break;
                    case AnimalStates.Dead:
                        SetDeadState();
                        break;
                    default:
                        Debug.LogError("Unhandled animal state!");
                        break;
                }
            }
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (!animator)
            Debug.LogError("No Animator found on the Animal!");
        if (!rb)
            Debug.LogError("No Rigidbody found on the Animal!");

        currentTarget = pointB;
        CurrentState = AnimalStates.Walking;

        switch (animalType)
        {
            case AnimalType.Cow:
                damage = 10; 
                break;

            case AnimalType.Cat:
                damage = 3;
                break;

            case AnimalType.Dog:
                damage = 5; 
                break;

            default:
                break; 
        }
    }

    void Update()
    {
        if (currentState == AnimalStates.Walking)
        {
            HandleWalking();
        }
    }

    private void HandleWalking()
    {
        if (currentTarget == null) return;
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, walkSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
        }
    }

    private void SetWalkingState()
    {
        Debug.Log("Setting Walking State");
        currentTarget = pointB;
    }

    private void SetIdleState()
    {
        currentTarget = null;
        Debug.Log("Setting Idle State");
    }

    private void SetFallingState()
    {
        Debug.Log("Setting Falling State");
    }

    private void SetDeadState()
    {
        Debug.Log("Setting Dead State");
        Destroy(gameObject, 2f);
    }

    public void SwitchState(AnimalStates state)
    {
        CurrentState = state; 
    }

    public void CauseDamageOnImpact(float velocity, int damage, HealthSystem healthSystem)
    {
        //int roundedVelocity = Mathf.RoundToInt(velocity);
        healthSystem.GetDamaged(damage);
    }

    public void DieUponImpact()
    { 
        CurrentState = AnimalStates.Dead;
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

        if (impactVelocity > deathVelocityThreshold)
        {
            Debug.Log("Animal died upon impact with velocity: " + impactVelocity);
            DieUponImpact();
        }
    }
}
