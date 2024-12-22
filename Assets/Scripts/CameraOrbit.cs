using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform player;            // Referencia al jugador
    [SerializeField] private float distance = 5f;         // Distancia desde la cámara al jugador
    [SerializeField] private float height = 2f;           // Altura de la cámara respecto al jugador
    [SerializeField] private float rotationSpeed = 5f;    // Velocidad de rotación de la cámara
    [SerializeField] private float followSpeed = 10f;     // Velocidad con la que la cámara sigue al jugador

    private float currentRotationX = 0f;  // Rotación de la cámara alrededor del eje X (vertical)
    private float currentRotationY = 0f;  // Rotación de la cámara alrededor del eje Y (horizontal)

    [SerializeField] private float maxYAngle = 80f;   // Ángulo máximo de rotación vertical (previene que la cámara se voltee)
    [SerializeField] private float minYAngle = -80f;  // Ángulo mínimo de rotación vertical

    void Start()
    {
        Quaternion cameraRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 forward = cameraRotation * Vector3.forward;
        Vector3 right = cameraRotation * Vector3.right;

    }
    void Update()
    {
        // Obtener el movimiento del ratón (solo en el eje X para rotación horizontal)
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        // Solo actualizamos la rotación horizontal (Y)
        currentRotationY += mouseX;  // Rotación horizontal (alrededor del eje Y)

        // No actualizamos la rotación en el eje X (evita rotación vertical)
        // currentRotationX -= mouseY;  // Esta línea se elimina para evitar la rotación vertical

        // Calcular la rotación y la posición de la cámara (sin rotación vertical)
        Quaternion rotation = Quaternion.Euler(0, currentRotationY, 0);  // Solo rotación horizontal (alrededor del eje Y)
        Vector3 targetPosition = player.position - rotation * Vector3.forward * distance;
        targetPosition.y = player.position.y + height;

        // Mover la cámara hacia la nueva posición suavemente
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Hacer que la cámara siempre mire al jugador
        transform.LookAt(player.position);
    }
}
