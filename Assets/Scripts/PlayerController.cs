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

    public Camera playerCamera;  // Referencia a la cámara
    public float cameraRotationSpeed = 5f; // Velocidad de rotación de la cámara
    public float cameraDistance = 5f; // Distancia de la cámara desde el jugador
    public float cameraHeight = 10f; // Altura de la cámara con respecto al jugador

    private float currentCameraRotationX = 0f;
    private float currentCameraRotationY = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialScale = transform.localScale;
        animator = GetComponent<Animator>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;  // Si no se asigna, buscar la cámara principal
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Control de rotación de la cámara con el ratón
        float mouseX = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed;

        currentCameraRotationX -= mouseY;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -50f, 50f);

        currentCameraRotationY += mouseX;

        // Aplicar rotación a la cámara
        Quaternion rotation = Quaternion.Euler(currentCameraRotationX, currentCameraRotationY, 0);
        Vector3 cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
        playerCamera.transform.position = transform.position + rotation * cameraOffset;
        playerCamera.transform.LookAt(transform.position);

        // Movimiento del jugador
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        // Obtener la rotación de la cámara para orientar el movimiento
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0;  // No queremos que la cámara afecte el movimiento vertical
        right.y = 0;

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;
        moveDirection.Normalize();

        // Mover el jugador en la dirección calculada
        transform.position += moveDirection * speed * Time.deltaTime;

        // Rotar el jugador para mirar hacia la dirección del movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Actualizar animaciones
        animator.SetFloat("XSpeed", horizontalInput);  // Movimiento horizontal
        animator.SetFloat("YSpeed", verticalInput);    // Movimiento vertical

        // Escalado con la rueda del ratón
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            Vector3 newScale = transform.localScale + Vector3.one * scrollInput * scaleSpeed;
            newScale = new Vector3(
                Mathf.Clamp(newScale.x, minScale.x, maxScale.x),
                Mathf.Clamp(newScale.y, minScale.y, maxScale.y),
                Mathf.Clamp(newScale.z, minScale.z, maxScale.z)
            );

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
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, resetSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = initialScale;
        resetCoroutine = null;
    }
}