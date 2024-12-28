using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class RepelAttract : MonoBehaviour
{
    public LayerMask layermask = LayerMask.GetMask("Animals", "DestroyedBuildings");
    public Transform rayOrigin; 
    public Vector3 rayDirection = Vector3.down; 
    public float rayLength = 10f; 
    public float moveSpeed = 5f; 
    public Transform ship; 
    public float repelDistance = 10f; 

    private Transform currentObject; 

    void Update()
    {
        Debug.DrawRay(rayOrigin.position, rayDirection.normalized * rayLength, Color.green);
        RaycastHit hit;
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(rayOrigin.position, rayDirection, out hit, rayLength, layermask))
            {
                Debug.Log($"Hit {hit.collider.name}");
                currentObject = hit.collider.transform;
                Vector3 targetPosition = ship.position;
                currentObject.position = Vector3.MoveTowards(currentObject.position, targetPosition, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(currentObject.position, targetPosition) < 0.1f)
                {
                    //Destroy when it reaches target --> Make it so that ++ points or whatever
                    currentObject = null;
                    Destroy(currentObject);
                }
            }
            
        }
        if (Input.GetMouseButton(1))
        {
            if (Physics.Raycast(rayOrigin.position, rayDirection, out hit, rayLength, layermask))
            {
                Debug.Log($"Hit {hit.collider.name}");
                currentObject = hit.collider.transform;
                Vector3 repelDirection = (currentObject.position - rayOrigin.position).normalized;
                Vector3 repelTarget = currentObject.position + repelDirection * repelDistance;
                currentObject.position = Vector3.MoveTowards(currentObject.position, repelTarget, moveSpeed * Time.deltaTime);
            }
            
        }
        else
        {
            currentObject = null;
        }
    }


    private void AttractObject(Transform objectToAttract)
    {
        Vector3 targetPosition = ship.position;
        currentObject.position = Vector3.MoveTowards(objectToAttract.position,targetPosition,moveSpeed * Time.deltaTime);
        if (Vector3.Distance(objectToAttract.position, targetPosition) < 0.1f)
        {
            //Destroy when it reaches target --> Make it so that ++ points or whatever
            currentObject = null;
            Destroy(objectToAttract);
        }
    }

    private void RepelObject(Transform objectToRepel)
    {
        Vector3 repelDirection = (objectToRepel.position - rayOrigin.position).normalized;
        Vector3 repelTarget = objectToRepel.position + repelDirection * repelDistance;
        objectToRepel.position = Vector3.MoveTowards(objectToRepel.position, repelTarget, moveSpeed * Time.deltaTime);
    }
}
