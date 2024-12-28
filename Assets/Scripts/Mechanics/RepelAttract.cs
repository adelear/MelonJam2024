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

    private Transform currentObject;
    private void Start()
    {
        timer = 0; 
    }
    void Update()
    {
        Debug.DrawRay(rayOrigin.position, rayDirection1.normalized * rayLength, Color.green);
        Debug.DrawRay(rayOrigin.position, rayDirection2.normalized * rayLength, Color.green);
        Debug.DrawRay(rayOrigin.position, rayDirection3.normalized * rayLength, Color.green);
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(rayOrigin.position, rayDirection1, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection2, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection3, out hit, rayLength, layermask))
            {
                Debug.Log($"Hit {hit.collider.name}");
                currentObject = hit.collider.transform;
                timer += Time.deltaTime; 
                AttractObject(currentObject, timer); 
            }
            else
            {
                if (currentObject == null) return; 
                if (Vector3.Distance(currentObject.position, ship.position) > 0.4f) currentObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (Physics.Raycast(rayOrigin.position, rayDirection1, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection2, out hit, rayLength, layermask) || Physics.Raycast(rayOrigin.position, rayDirection3, out hit, rayLength, layermask))
            {
                Debug.Log($"Hit {hit.collider.name}");
                currentObject = hit.collider.transform;
                RepelObject(currentObject); 
            }
            
        }
        else
        {
            if (currentObject != null)
            {
                currentObject.GetComponent<Rigidbody>().useGravity = true;
                currentObject = null;
            }
            
        }
    }


    private void AttractObject(Transform objectToAttract,float time)
    {
        Vector3 targetPosition = ship.position;
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
            if (objectToAttract.GetComponent<Animal>())
            {
                currentObject.gameObject.SetActive(false); 
                currentObject = null;
                Debug.Log("Destroyed Object"); 
            }
        }
    }

    private void RepelObject(Transform objectToRepel)
    {
        Rigidbody rb = objectToRepel.GetComponent<Rigidbody>();
        rb.AddForce(0, repelSpeed, 0, ForceMode.Impulse); 
        currentObject.GetComponent<Rigidbody>().useGravity = true;
    }
}
