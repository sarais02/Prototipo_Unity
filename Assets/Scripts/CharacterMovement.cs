using System.Collections; 
using System.Collections.Generic;  
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 50f;


    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        if (movementDirection.magnitude > 1f)
        {
            movementDirection.Normalize();
        }


        transform.position = transform.position + movementDirection * speed * Time.deltaTime;


        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed * Time.deltaTime);
        }
    }
}
