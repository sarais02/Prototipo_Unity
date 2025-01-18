using System.Diagnostics;
using System.Collections;
using UnityEngine;

public class RaycastVision : MonoBehaviour
{
    private AudioSource audioSource;
    private AudioClip clip;
    private float viewAngle = 45f; // Ángulo del cono de visión
    private float viewDistance = 6f; // Distancia máxima del cono de visión
    [SerializeField] private LayerMask targetMask; // Capa del jugador
    [SerializeField] private LayerMask obstacleMask; // Capa de obstáculos

    void Start()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
        clip = Resources.Load<AudioClip>("jump-scare-sound-2-82831");
    }

    void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (Collider target in targetsInView)
        {
            Vector3 directionToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

                // Verifica si hay algún obstáculo entre el enemigo y el jugador
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    //Sonido
                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(clip);
                        target.GetComponent<PlayerRespawn>()?.KillPlayer(2f);
                    }
                    break;
                }
            }
        }
    }
}
