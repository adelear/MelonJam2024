using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    private Rigidbody rb;
    private Vector2 input;

    public float tiltAngle = 15f; 
    public float tiltSpeed = 5f; 
    public float recenterSpeed = 2f; 
    public float layerDistance = 5f; 
    public int maxLayers = 2; 

    private float currentTilt = 0f; 
    private int currentLayer = 0;

    RepelAttract repelAttract; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        repelAttract = GetComponent<RepelAttract>(); 
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        //input.Normalize(); // to match speed with diagonal

        float targetTilt = -input.x * tiltAngle;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        transform.rotation = Quaternion.Euler(currentTilt - 90f, 90f, 90f);
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            currentLayer = Mathf.Clamp(currentLayer + (scrollInput > 0 ? 1 : -1), 0, maxLayers);
        }

        float targetZPosition = currentLayer * layerDistance;
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, targetZPosition, Time.deltaTime * 2f)); 
        if (repelAttract.currentObject != null && repelAttract.isAttracting)
        {
            repelAttract.currentObject.position = new Vector3(repelAttract.currentObject.position.x, repelAttract.currentObject.position.y, Mathf.Lerp(transform.position.z, targetZPosition, Time.deltaTime * 1f)); 
        }
    }
     
    void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;

        if (Mathf.Abs(input.x) < 0.01f)
        {
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * recenterSpeed);
            transform.rotation = Quaternion.Euler(90f, 90f, 90f);
        }

        rb.AddForce(Physics.gravity * 2, ForceMode.Acceleration); 
    }
}
