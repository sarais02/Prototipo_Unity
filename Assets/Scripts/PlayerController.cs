using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 0.2f;  // Velocidad de escalado
    [SerializeField] private float resetSpeed = 8.0f;
    [SerializeField] private Vector3 minScale = new Vector3(0.1f, 0.1f, 0.1f);  // Escala mínima permitida
    [SerializeField] private Vector3 maxScale = new Vector3(5f, 5f, 5f);  // Escala máxima permitida
    [SerializeField] private float time = 3f;

    private GameManager gm;
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
        gm = GameManager.GetInstance();
        initialScale = transform.localScale;
        initialPos = transform.position;
        animator = GetComponent<Animator>();
        size = GetComponent<MeshRenderer>().bounds.size;

        if (playerCamera == null)
        {
            playerCamera = Camera.main;  // Si no se asigna, buscar la cámara principal
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gm && !gm.IsMenu())
        {
            CameraMovement();
            ScrollScale();
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * verticalInput + right * horizontalInput;

        if (moveDirection.magnitude > 0.01f)
        {
            moveDirection.Normalize();
        }

        transform.position += moveDirection * speed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        // Update animations
        if (animator != null)
        {
            animator.SetFloat("XSpeed", horizontalInput);  // Movimiento horizontal
            animator.SetFloat("YSpeed", verticalInput);    // Movimiento vertical
        }

        // Check if "F" key is pressed to trigger the throw animation
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (animator != null)
            {
                animator.SetTrigger("Throw");  // Activate the "Throw" trigger
            }
        }
    }

    // Handles the camera movement and rotation
    private void CameraMovement()
    {

        // Control de rotación de la cámara con el ratón
        float mouseX = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        if (gm.IsTutorial())
        {
            float mouseY = Input.GetAxis("Mouse Y") * cameraRotationSpeed;
            currentCameraRotationX -= mouseY;
        }
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -50f, 50f);

        currentCameraRotationY += mouseX;

        // Aplicar rotación a la cámara
        Quaternion rotation = Quaternion.Euler(currentCameraRotationX, currentCameraRotationY, 0);
        Vector3 cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
        playerCamera.transform.position = transform.position + rotation * cameraOffset;
        playerCamera.transform.LookAt(transform.position);
    }

    // Handles scaling with the mouse scroll wheel
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
                Mathf.Clamp(newScale.z, minScale.z, maxScale.z));

            transform.localScale = newScale;

            if (!isScaling)
            {
                isScaling = true;
                timeSinceLastScaleChange = Time.time; // Guarda el momento inicial del escalado
                StartCoroutine(ResetScaleAfterDelay());
            }
        }
    }

    // Coroutine to reset the scale after a delay
    IEnumerator ResetScaleAfterDelay()
    {
        while (Time.time < timeSinceLastScaleChange + time)
        {
            yield return null; // Wait until the delay time has passed
        }

        while (Vector3.Distance(transform.localScale, initialScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, initialScale, resetSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localScale = initialScale;
        isScaling = false;
    }

    // Detects collisions with the "Water" object and respawns the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Water"))
        {
            Respawn();
        }
    }

    // Respawns the player to their initial position
    private void Respawn()
    {
        Debug.Log("Respawn");
        transform.position = initialPos;
    }
}
