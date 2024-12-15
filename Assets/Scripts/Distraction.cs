using System.Collections;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    [SerializeField] private float distractionRadius;
    [SerializeField] private float investigationTime;
    [SerializeField] private Transform auxTransform;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            TriggerDistraction();
        }
    }

    void TriggerDistraction()
    {
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, distractionRadius);

        foreach (Collider col in nearbyEnemies)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemyAI enemy = col.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.Investigate(auxTransform.position);
                }
            }
        }
        Destroy(gameObject, 2.5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distractionRadius);
    }
}