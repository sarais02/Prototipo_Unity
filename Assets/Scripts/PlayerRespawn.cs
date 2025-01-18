using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private bool isDead = false;

    public void KillPlayer(float time)
    {
        if (isDead) return;
        isDead = true;
        Debug.Log("Jugador ha muerto");
        Invoke(nameof(Respawn), time);
    }

    public void Respawn()
    {
        isDead = false;
        // Reposiciona al jugador en el último checkpoint alcanzado
        transform.position = CheckpointManager.Instance.GetCheckpointPosition();
    }
}
