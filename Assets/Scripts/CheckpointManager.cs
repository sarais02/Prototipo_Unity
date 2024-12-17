using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector3 lastCheckpointPosition;

    private void Awake()
    {
        // Asegura que solo exista una instancia del CheckpointManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Guarda la posición del checkpoint
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
    }

    // Obtiene la posición del último checkpoint alcanzado
    public Vector3 GetCheckpointPosition()
    {
        return lastCheckpointPosition;
    }
}
