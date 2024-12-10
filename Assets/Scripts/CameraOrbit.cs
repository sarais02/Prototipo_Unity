using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform player;            // Referencia al jugador
    public float distance = 5f;         // Distancia desde la cámara al jugador
    public float height = 2f;           // Altura de la cámara respecto al jugador
    public float rotationSpeed = 5f;    // Velocidad de rotación de la cámara
    public float followSpeed = 10f;     // Velocidad con la que la cámara sigue al jugador

    private float currentRotationX = 0f;  // Rotación de la cámara alrededor del eje X (vertical)
    private float currentRotationY = 0f;  // Rotación de la cámara alrededor del eje Y (horizontal)

    public float maxYAngle = 80f;   // Ángulo máximo de rotación vertical (previene que la cámara se voltee)
    public float minYAngle = -80f;  // Ángulo mínimo de rotación vertical

    void Update()
    {
        // Obtener el movimiento del ratón
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Actualizar la rotación de la cámara en los ejes X e Y
        currentRotationY += mouseX;  // Rotación horizontal (alrededor del eje Y)
        currentRotationX -= mouseY;  // Rotación vertical (alrededor del eje X)

        // Limitar la rotación vertical para evitar que la cámara se voltee completamente
        currentRotationX = Mathf.Clamp(currentRotationX, minYAngle, maxYAngle);

        // Calcular la rotación y la posición de la cámara
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 targetPosition = player.position - rotation * Vector3.forward * distance;
        targetPosition.y = player.position.y + height;

        // Mover la cámara hacia la nueva posición suavemente
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Hacer que la cámara siempre mire al jugador
        transform.LookAt(player.position);
    }
}