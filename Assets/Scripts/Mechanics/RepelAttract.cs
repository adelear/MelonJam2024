using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
 
public class RepelAttract : MonoBehaviour
{
    public LayerMask layermask;
    public Transform rayOrigin; 
    public Vector3 rayDirection1 = Vector3.down;
    public Vector3 rayDirection2 = Vector3.down;
    public Vector3 rayDirection3 = Vector3.down;
    public float rayLength = 10f;  
    public float moveSpeed = 5f; 
    public Transform ship; 
    public float repelSpeed = 10f;
    float timer;

    public bool isAttracting; 

    public Transform currentObject;

    [SerializeField] private GameObject attractBeam;
    [SerializeField] private GameObject repelBeam;
    [SerializeField] private AudioClip beamSound;
    [SerializeField] private AudioClip collectSound; 
    private bool beamSoundPlaying;
    private bool hasPlayedAbductedAudio; 
    private bool hasPlayedRepelAudio; 
    

    private SphereCollider sc; 
    private void Start()
    {
        timer = 0; 
        sc = GetComponent<SphereCollider>();
        sc.enabled = false;
        repelBeam.SetActive(false);
        attractBeam.SetActive(false);

        hasPlayedAbductedAudio = false;
        hasPlayedAbductedAudio = false; 
    }
    void Update()
    {
        if (GameManager.Instance.GetGameState() != GameManager.GameState.GAME) return; 
        Debug.DrawRay(rayOrigin.position, rayDirection1.normalized * rayLength, Color.green);
        Debug.DrawRay(rayOrigin.position, rayDirection2.normalized * rayLength, Color.green);
        Debug.DrawRay(rayOrigin.position, rayDirection3.normalized * rayLength, Color.green);
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            if (!beamSoundPlaying)
            {
                AudioManager.Instance.PlayOneShotWithRandomPitch(beamSound, false, 0.8f, 1.2f, 0.1f);  
                beamSoundPlaying = true; 
            }
            repelBeam.SetActive(false);
            attractBeam.SetActive(true);
            if (Physics.Raycast(rayOrigin.position, rayDirection1, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection2, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection3, out hit, rayLength, layermask))
            {
                Vector3 rayOriginFlat = new Vector3(rayOrigin.position.x, rayOrigin.position.y, 0f);
                Vector3 hitPointFlat = new Vector3(hit.point.x, hit.point.y, 0f);

                // Check distance in the x-y plane
                if (Vector3.Distance(rayOriginFlat, hitPointFlat) <= rayLength)
                {
                    Debug.Log($"Hit {hit.collider.name}");
                    currentObject = hit.collider.transform;
                    timer += Time.deltaTime;
                    AttractObject(currentObject, timer);
                    sc.enabled = true;
                }
            }
            else
            {
                if (currentObject == null) return; 
                if (Vector3.Distance(currentObject.position, ship.position) > 0.4f) currentObject.GetComponent<Rigidbody>().useGravity = true;
                sc.enabled = false;
            }
            
        }
        else if (Input.GetMouseButton(1))
        {
            repelBeam.SetActive(true);
            attractBeam.SetActive(false);
            if (!beamSoundPlaying)
            {
                AudioManager.Instance.PlayOneShotWithRandomPitch(beamSound, false, 0.8f, 1.2f, 0.1f); 
                beamSoundPlaying = true;
            }
            if (Physics.Raycast(rayOrigin.position, rayDirection1, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection2, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection3, out hit, rayLength, layermask))
            {
                Debug.Log($"Hit {hit.collider.name}");
                currentObject = hit.collider.transform;
                RepelObject(currentObject);
                sc.enabled = false;
            }
        }
        else
        {
            repelBeam.SetActive(false);
            attractBeam.SetActive(false);
            if (currentObject != null)
            {
                currentObject.GetComponent<Rigidbody>().useGravity = true;
                if (currentObject.gameObject.GetComponent<Animal>())
                {
                    Animal animal = currentObject.gameObject.GetComponent<Animal>();
                    hasPlayedRepelAudio = false;
                    hasPlayedAbductedAudio = false; 
                } 
                currentObject = null;
                sc.enabled = false;
                isAttracting = false; 
            }
            beamSoundPlaying = false; 
        }
    }

     
    private void AttractObject(Transform objectToAttract,float time)
    {
        
        Vector3 targetPosition = ship.position;
        isAttracting = true; 
        if (Vector3.Distance(objectToAttract.position, targetPosition) > 0.4f)
        {
            currentObject.position = Vector3.MoveTowards(objectToAttract.position, targetPosition, moveSpeed * Time.deltaTime);
            currentObject.GetComponent<Rigidbody>().useGravity = false;
        }
        if (Vector3.Distance(objectToAttract.position, targetPosition) < 0.4f)
        {
            objectToAttract.position = targetPosition; 
            //objectToAttract.position = new Vector3(targetPosition.x, Mathf.Cos(time * (moveSpeed/2)/ Mathf.PI) * (targetPosition.y - 2), targetPosition.z); 
            //Destroy when it reaches target --> Make it so that ++ points or whatever ONLY IF ITS AN ANIMAL 
            
        }

        if (currentObject.gameObject.GetComponent<Animal>() && !hasPlayedAbductedAudio)
        {
            Animal animal = currentObject.gameObject.GetComponent<Animal>();
            if (animal.abductSound!= null) AudioManager.Instance.PlayOneShotWithRandomPitch(animal.abductSound, false, 1.2f, 2f, animal.volume); 
            hasPlayedAbductedAudio = true;
            hasPlayedRepelAudio = false; 
        }
    }

    private void RepelObject(Transform objectToRepel)
    {
        Rigidbody rb = objectToRepel.GetComponent<Rigidbody>();
        rb.AddForce(0, repelSpeed, 0, ForceMode.Impulse); 
        currentObject.GetComponent<Rigidbody>().useGravity = true;
        isAttracting = false;

        if (currentObject.gameObject.GetComponent<Animal>() && !hasPlayedRepelAudio)
        {
            Animal animal = currentObject.gameObject.GetComponent<Animal>();
            if (animal.abductSound != null) AudioManager.Instance.PlayOneShotWithRandomPitch(animal.fallSound, false, 1.2f, 2f, animal.volume);
            hasPlayedRepelAudio = true;
            hasPlayedAbductedAudio = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Animal>())
        {
            if (other.GetComponent<Animal>().CurrentState == AnimalStates.Dead) return; 
            Debug.Log("Collected an animal");
            int score = 0; 
            switch (other.GetComponent<Animal>().animalType)
            {
                case AnimalType.Cow: score = 5; break;
                case AnimalType.Dog: score = 3; break;
                case AnimalType.Cat: score = 2; break;
                case AnimalType.Human:
                    score = Random.Range(-5, -1);
                    MilitaryManager.Instance.AddMilitaryPoints(5); 
                    break; 
                default: score = 0; break; 
            }
            GameManager.Instance.Score += score;
            AudioManager.Instance.PlayOneShotWithRandomPitch(collectSound, false, 0.8f, 1.2f, 1f); 
            Destroy(other.gameObject); 
        }
    }
}
