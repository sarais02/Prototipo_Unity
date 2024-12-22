using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform player;            // Referencia al jugador
    [SerializeField] float cameraRotationSpeed = 5f; // Velocidad de rotación de la cámara
    [SerializeField] float cameraDistance = 5f; // Distancia de la cámara desde el jugador

    [SerializeField] float cameraHeight = 10f; // Altura de la cámara con respecto al jugador
    [SerializeField] private PauseSystem pauseMenu;
    private GameManager gm;
    private float currentCameraRotationX = 0f;
    private float currentCameraRotationY = 0f;

    void Start()
    {
        gm = GameManager.GetInstance();
        //if (gm && !gm.IsMenu())
        //{
        //    CameraMovement();
        //}
    }

    void Update()
    {
        if (gm && !gm.IsMenu()) CameraMovement();
        // Hacer que la cámara siempre mire al jugador
        //transform.LookAt(player.position);
    }

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
        if (!pauseMenu.IsPaused())
        {
            Quaternion rotation = Quaternion.Euler(currentCameraRotationX, currentCameraRotationY, 0);
            Vector3 cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
            transform.position = player.transform.position + rotation * cameraOffset;
            transform.LookAt(player.transform.position);
        }
    }
}
