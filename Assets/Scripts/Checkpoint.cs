using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Guarda la posición del checkpoint en el sistema de reinicio del jugador
            CheckpointManager.Instance.SetCheckpoint(transform.position);
        }
    }
}
