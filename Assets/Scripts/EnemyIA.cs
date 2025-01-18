using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Animator
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip clip;

    // Enumeración para los tipos de enemigos
    public enum EnemyType
    {
        LookSideToSide,
        PatrolWaypoints,
        Rotate360,
        LookAndRest
    }

    [SerializeField] private EnemyType enemyType; // Tipo de enemigo
    [SerializeField] private float rotationSpeed; // Velocidad de rotación para todos
    [SerializeField] private Transform[] waypoints; // Waypoints para patrulla
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float lookDuration; // Tiempo mirando para LookAndRest
    [SerializeField] private float restDuration; // Tiempo descansando para LookAndRest
    [SerializeField] private Transform visionCone; 

    [SerializeField] private float investigationSpeed;
    [SerializeField] private float investigationTime;
    private bool isInvestigating = false;

    private Transform currentWaypoint;
    private int currentWaypointIndex = 0;
    private bool isResting = false; // Usado solo para LookAndRest
    private float lookTimer = 0f; // Temporizador para LookAndRest

    [SerializeField] private float viewAngle = 45f; // Ángulo del cono de visión
    [SerializeField] private float viewDistance = 10f; // Distancia máxima del cono de visión
    [SerializeField] private LayerMask targetMask; // Capa del jugador
    [SerializeField] private LayerMask obstacleMask; // Capa de obstáculos

    private Transform playerTransform; // Referencia al jugador
    private GameObject player;
    private bool playerDetected = false; // Estado de detección


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        animator = GetComponent<Animator>();
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.1f;
        clip = Resources.Load<AudioClip>("jump-scare-sound-2-82831");
        
        
        

        // Si hay waypoints, selecciona el primero
        if (waypoints.Length > 0)
            currentWaypoint = waypoints[0];
    }

    private void Update()
    {
        if (enemyType != EnemyType.LookSideToSide) DetectPlayer();
        if (playerDetected)
        {
            player.GetComponent<PlayerRespawn>()?.KillPlayer(2f);            
        }
        if (!isInvestigating)
        {
            switch (enemyType)
            {
                case EnemyType.LookSideToSide:
                    LookSideToSide();
                    break;

                case EnemyType.PatrolWaypoints:
                    PatrolWaypoints();
                    break;

                case EnemyType.Rotate360:
                    Rotate360();
                    break;

                case EnemyType.LookAndRest:
                    LookAndRest();
                    break;
            }
        }

    }

    private void LookSideToSide()
    {
        // Oscila entre -135 y -45 grados
        float rotationAngle = Mathf.PingPong(Time.time * rotationSpeed, 90f) - 135f;
        visionCone.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }


    private void PatrolWaypoints()
    {
        if (currentWaypoint == null) return;

        Vector3 direction = (currentWaypoint.position - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;

        // Rotar hacia el waypoint actual
        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 360 * Time.deltaTime);

        // Comprobar si se alcanzó el waypoint actual
        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.5f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            currentWaypoint = waypoints[currentWaypointIndex];
        }
    }

    private void Rotate360()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void LookAndRest()
    {
        //Descansando
    }

    public void Investigate(Vector3 distractionPosition) //Solo es public para llamarlo desde Distraction
    {

        if (!isInvestigating)
        {
            switch (enemyType)
            {
                case EnemyType.LookSideToSide:
                    StartCoroutine(InvestigationLookSideToSide());
                    break;

                case EnemyType.PatrolWaypoints:
                    StartCoroutine(InvestigationPatrolRoutine(distractionPosition));
                    break;

                case EnemyType.Rotate360:
                    StartCoroutine(RotateTowardsDirection(distractionPosition));
                    break;
            }
        }
    }

    private IEnumerator InvestigationLookSideToSide()
    {
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator InvestigationPatrolRoutine(Vector3 position)
    {
        isInvestigating = true;
        Debug.Log("Investing");

        while (Vector3.Distance(transform.position, position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, investigationSpeed * Time.deltaTime);

            Vector3 direction = (position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            yield return null;
        }

        animator.SetBool("Distraction", true);
        yield return new WaitForSeconds(investigationTime);

        isInvestigating = false;
        animator.SetBool("Distraction", false);
        currentWaypoint = waypoints[currentWaypointIndex];
    }

    private IEnumerator RotateTowardsDirection(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Dibuja en la vista del editor
    private void OnDrawGizmosSelected()
    {
        if (waypoints != null && waypoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.DrawSphere(waypoints[i].position, 0.2f);
                if (i < waypoints.Length - 1)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                }
                else
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position); // Cerrar ciclo
                }
            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 viewAngleA = DirectionFromAngle(-viewAngle / 2);
        Vector3 viewAngleB = DirectionFromAngle(viewAngle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewDistance);

        if (playerDetected)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerTransform.position);
        }
    }

    private Vector3 DirectionFromAngle(float angleInDegrees)
    {
        float angle = angleInDegrees + transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }


    private void DetectPlayer()
    {
        playerDetected = false;
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
                    }
                    playerDetected = true;
                    Debug.Log("Player detected!");
                    break;
                }
            }
        }

        if (!playerDetected)
        {
            //Debug.Log("Player not in sight.");
        }
    }
}
