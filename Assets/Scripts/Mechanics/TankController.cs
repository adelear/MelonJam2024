using System.Collections;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public enum TankState { MoveToPlayer, Fire, Idle }
    private TankState currentState = TankState.Idle;

    [Header("Tank Movement")]
    public float moveSpeed = 2f; 
    public float stoppingDistance = 10f; 

    [Header("Barrel and Shooting")]
    public Transform barrel; 
    public Transform player; 
    public Transform firePoint; 
    public GameObject projectilePrefab; 
    public float fireCooldown = 2f;
    public float barrelRotationSpeed = 5f; 
    public float projectileSpeed = 10f; 

    [Header("Idle Settings")]
    public float idleTime = 2f; 

    private float fireTimer = 0f;
    private float idleTimer = 0f;

    [SerializeField] private AudioClip tankShoot; 

    private void Start()
    {
        currentState = TankState.MoveToPlayer;
        player = GameObject.FindGameObjectWithTag("Player").transform; 
    }

    private void Update()
    {
        RotateBarrel();

        switch (currentState)
        {
            case TankState.MoveToPlayer:
                MoveToPlayer();
                break;
            case TankState.Fire:
                Fire();
                break;
            case TankState.Idle:
                Idle();
                break;
        }
    }

    private void MoveToPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += new Vector3(direction.x,0f,0f) * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Switch to Fire state when within range
            currentState = TankState.Fire;
            fireTimer = 0f;
        }
    }

    private void Fire()
    {
        if (fireTimer == 0f)
        {
            FireProjectile();
        }

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCooldown)
        {
            fireTimer = 0f;

            // After firing, transition to Idle state
            currentState = TankState.Idle;
            idleTimer = 0f; 
        }
    }

    private void Idle()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime)
        {
            // After idle time, switch back to MoveToPlayer state
            currentState = TankState.MoveToPlayer;
        }
    }

    private void RotateBarrel()
    {
        if (barrel == null || player == null) return;
        Vector3 direction = player.position - barrel.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        barrel.rotation = Quaternion.Lerp(barrel.rotation, targetRotation, Time.deltaTime * barrelRotationSpeed);
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null) return;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, barrel.rotation);
        AudioManager.Instance.PlayOneShotWithRandomPitch(tankShoot, false, 0.8f, 1.2f, 0.6f);
        // Apply velocity to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 velocity = barrel.TransformDirection(Vector3.forward * projectileSpeed);
            rb.velocity = velocity;
        }
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(firePoint.position, firePoint.position + barrel.right * 2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        float impactVelocity = collision.relativeVelocity.magnitude;

        if (impactVelocity > 20f)
        {
            Debug.Log("Animal died upon impact with velocity: " + impactVelocity);
            gameObject.GetComponent<HealthSystem>().GetDamaged(10); 
        }
    }
}
