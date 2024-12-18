using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.2f;  // Velocidad de escalado
    [SerializeField] private float resetSpeed = 8.0f;
    [SerializeField] private Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);  // Escala mínima permitida
    [SerializeField] private Vector3 maxScale = new Vector3(5f, 5f, 5f);  // Escala máxima permitida
    [SerializeField] private float time = 3f;

    GameManager gm;
    public Vector3 size;
    private Vector3 initialScale;

    public float speed = 5f;
    public float rotationSpeed = 50f;
    private Animator animator;

    public Camera playerCamera;  // Referencia a la cámara
    public float cameraRotationSpeed = 5f; // Velocidad de rotación de la cámara
    public float cameraDistance = 5f; // Distancia de la cámara desde el jugador
    public float cameraHeight = 10f; // Altura de la cámara con respecto al jugador

    private float currentCameraRotationX = 0f;
    private float currentCameraRotationY = 0f;

    private float timeSinceLastScaleChange;
    private bool isScaling = false;
    private Vector3 initialPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gm=GameManager.GetInstance();
        initialScale = transform.localScale;
        initialPos = transform.position;
        animator = GetComponent<Animator>();
        size = GetComponent<MeshRenderer>().bounds.size;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;  // Si no se asigna, buscar la cámara principal
        }
        
        //Vector3 savedPos = new Vector3(
        //    PlayerPrefs.GetFloat("PlayerPosX"),
        //    PlayerPrefs.GetFloat("PlayerPosY"),
        //    PlayerPrefs.GetFloat("PlayerPosZ"));

        //if(savedPos != Vector3.zero)
        //    transform.position = savedPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm && !gm.IsMenu())
        {
            CameraMovement();

            ScrollScale();
        }

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
        //PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
        //PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
        //PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);

        // Rotar el jugador para mirar hacia la dirección del movimiento
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Actualizar animaciones
        if (animator != null)
        {
            animator.SetFloat("XSpeed", horizontalInput);  // Movimiento horizontal
            animator.SetFloat("YSpeed", verticalInput);    // Movimiento vertical
        }

    }
    
    private void CameraMovement()
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
    }
    private void ScrollScale()
    {
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

            if (!isScaling)
            {
                isScaling = true;
                timeSinceLastScaleChange = Time.time; // Guarda el momento inicial del escalado
                StartCoroutine(ResetScaleAfterDelay());
            }
        }
    }
    IEnumerator ResetScaleAfterDelay()
    {
        while (Time.time < timeSinceLastScaleChange + time)
        {
            yield return null; // Espera hasta que se cumpla el tiempo de retraso
        }

        while (Vector3.Distance(transform.localScale, initialScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, resetSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = initialScale;
        isScaling = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            Respawn();
        }
    }
    private void Respawn()
    {
        Debug.Log("Respawn");
        transform.position = initialPos;
    }
}