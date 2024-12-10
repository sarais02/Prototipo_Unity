using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform player;            // Referencia al jugador
    public float distance = 5f;         // Distancia desde la c�mara al jugador
    public float height = 2f;           // Altura de la c�mara respecto al jugador
    public float rotationSpeed = 5f;    // Velocidad de rotaci�n de la c�mara
    public float followSpeed = 10f;     // Velocidad con la que la c�mara sigue al jugador

    private float currentRotationX = 0f;  // Rotaci�n de la c�mara alrededor del eje X (vertical)
    private float currentRotationY = 0f;  // Rotaci�n de la c�mara alrededor del eje Y (horizontal)

    public float maxYAngle = 80f;   // �ngulo m�ximo de rotaci�n vertical (previene que la c�mara se voltee)
    public float minYAngle = -80f;  // �ngulo m�nimo de rotaci�n vertical

    void Update()
    {
        // Obtener el movimiento del rat�n
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Actualizar la rotaci�n de la c�mara en los ejes X e Y
        currentRotationY += mouseX;  // Rotaci�n horizontal (alrededor del eje Y)
        currentRotationX -= mouseY;  // Rotaci�n vertical (alrededor del eje X)

        // Limitar la rotaci�n vertical para evitar que la c�mara se voltee completamente
        currentRotationX = Mathf.Clamp(currentRotationX, minYAngle, maxYAngle);

        // Calcular la rotaci�n y la posici�n de la c�mara
        Quaternion rotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
        Vector3 targetPosition = player.position - rotation * Vector3.forward * distance;
        targetPosition.y = player.position.y + height;

        // Mover la c�mara hacia la nueva posici�n suavemente
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Hacer que la c�mara siempre mire al jugador
        transform.LookAt(player.position);
    }
}