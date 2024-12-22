using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Transform player;            // Referencia al jugador

    void Start()
    {

    }

    void Update()
    {

        // Hacer que la c�mara siempre mire al jugador
        transform.LookAt(player.position);
    }
}
