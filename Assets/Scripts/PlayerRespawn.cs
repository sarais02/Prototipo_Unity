using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool isDead = false;

    public void KillPlayer()
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Jugador ha muerto");
        Invoke(nameof(Respawn), 2f); // 2 segundos de retraso antes del respawn
    }

    public void Respawn()
    {
        isDead = false;
        // Reposiciona al jugador en el último checkpoint alcanzado
        transform.position = CheckpointManager.Instance.GetCheckpointPosition();
    }
}
