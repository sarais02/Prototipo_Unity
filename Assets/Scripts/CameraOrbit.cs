
using System.Collections;
using UnityEngine;

public class CameraOrbit : MonoBehaviour

{

    public Transform player;      
    public float distance = 5f;    
    public float height = 2f;     
    public float rotationSpeed = 5f; 
    public float followSpeed = 10f; 

    private float currentRotation = 0f;

    void Update()
    {
       
        float horizontalInput = Input.GetAxis("Horizontal");

       
        currentRotation += horizontalInput * rotationSpeed * Time.deltaTime;

        
        Vector3 targetPosition = player.position - new Vector3(0, 0, distance);
        targetPosition.y = player.position.y + height;

      
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

     
        transform.LookAt(player.position);
    }
}
