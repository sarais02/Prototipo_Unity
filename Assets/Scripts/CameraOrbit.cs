using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform player;            // Referencia al jugador
    [SerializeField] private float distance = 5f;         // Distancia desde la c�mara al jugador
    [SerializeField] private float height = 2f;           // Altura de la c�mara respecto al jugador
    [SerializeField] private float rotationSpeed = 5f;    // Velocidad de rotaci�n de la c�mara
    [SerializeField] private float followSpeed = 10f;     // Velocidad con la que la c�mara sigue al jugador

    private float currentRotationX = 0f;  // Rotaci�n de la c�mara alrededor del eje X (vertical)
    private float currentRotationY = 0f;  // Rotaci�n de la c�mara alrededor del eje Y (horizontal)

    [SerializeField] private float maxYAngle = 80f;   // �ngulo m�ximo de rotaci�n vertical (previene que la c�mara se voltee)
    [SerializeField] private float minYAngle = -80f;  // �ngulo m�nimo de rotaci�n vertical

    void Start()
    {
        Quaternion cameraRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        Vector3 forward = cameraRotation * Vector3.forward;
        Vector3 right = cameraRotation * Vector3.right;

    }
    void Update()
    {
        // Obtener el movimiento del rat�n (solo en el eje X para rotaci�n horizontal)
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;

        // Solo actualizamos la rotaci�n horizontal (Y)
        currentRotationY += mouseX;  // Rotaci�n horizontal (alrededor del eje Y)

        // No actualizamos la rotaci�n en el eje X (evita rotaci�n vertical)
        // currentRotationX -= mouseY;  // Esta l�nea se elimina para evitar la rotaci�n vertical

        // Calcular la rotaci�n y la posici�n de la c�mara (sin rotaci�n vertical)
        Quaternion rotation = Quaternion.Euler(0, currentRotationY, 0);  // Solo rotaci�n horizontal (alrededor del eje Y)
        Vector3 targetPosition = player.position - rotation * Vector3.forward * distance;
        targetPosition.y = player.position.y + height;

        // Mover la c�mara hacia la nueva posici�n suavemente
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Hacer que la c�mara siempre mire al jugador
        transform.LookAt(player.position);
    }
}
