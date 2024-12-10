using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.2f;  // Velocidad de escalado
    [SerializeField] private float resetSpeed = 8.0f;
    [SerializeField] private Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);  // Escala mínima permitida
    [SerializeField] private Vector3 maxScale = new Vector3(5f, 5f, 5f);  // Escala máxima permitida
    [SerializeField] private float time = 3f;

    private Vector3 initialScale;
    private Coroutine resetCoroutine;

    public float speed = 5f;
    public float rotationSpeed = 50f;
    private Animator animator;
    public Camera playerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = transform.localScale;
        animator = GetComponent<Animator>();
    }
    

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);



        //if (movementDirection.magnitude > 1f)
        //{
        //    movementDirection.Normalize();
        //}
        movementDirection.Normalize();

        transform.position = transform.position + movementDirection * speed * Time.deltaTime;


        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDirection), rotationSpeed * Time.deltaTime);
        }

        animator.SetFloat("XSpeed", horizontalInput);  // Horizontal movement
        animator.SetFloat("YSpeed", verticalInput);    // Vertical movement

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (movementDirection.magnitude > 0)
        {
            // Obtener la rotación de la cámara para orientar el movimiento
            Vector3 forward = playerCamera.transform.forward; // Dirección de la cámara hacia adelante
            Vector3 right = playerCamera.transform.right;     // Dirección de la cámara hacia la derecha

            // Hacer que la dirección del movimiento esté basada en la cámara
            forward.y = 0;  // Asegurarse de que no se incluyan rotaciones en el eje Y (no afecte la altura)
            right.y = 0;

            // Calcular el movimiento final basado en las entradas
            Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
            moveDirection.Normalize();  // Normalizar la dirección

            // Mover el jugador en la dirección calculada
            transform.position += moveDirection * speed * Time.deltaTime;
        }

            if (scrollInput != 0)
        {
            // Calcula la nueva escala
            Vector3 newScale = transform.localScale + Vector3.one * scrollInput * scaleSpeed;

            // Limita la escala dentro del rango permitido
            newScale = new Vector3(
                Mathf.Clamp(newScale.x, minScale.x, maxScale.x),
                Mathf.Clamp(newScale.y, minScale.y, maxScale.y),
                Mathf.Clamp(newScale.z, minScale.z, maxScale.z)
            );

            // Aplica la nueva escala al objeto
            transform.localScale = newScale;

            if (resetCoroutine != null)
            {
                StopCoroutine(resetCoroutine);
            }
            resetCoroutine = StartCoroutine(ResetScaleAfterDelay());
        }
    }

    IEnumerator ResetScaleAfterDelay()
    {
        yield return new WaitForSeconds(time);

        while (Vector3.Distance(transform.localScale, initialScale) > 0.01f)
        {
            // Lerp para volver suavemente a la escala original
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, resetSpeed * Time.deltaTime);
            yield return null; // Espera un frame
        }

        transform.localScale = initialScale; // Asegura que se establece exactamente la escala original
        resetCoroutine = null; // Libera la referencia a la corutina
    }
}

