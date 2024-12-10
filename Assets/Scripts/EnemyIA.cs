using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Animator
    private Animator animator;

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

    [SerializeField] private float investigationSpeed;
    [SerializeField] private float investigationTime;
    private bool isInvestigating = false;

    private Transform currentWaypoint;
    private int currentWaypointIndex = 0;
    private bool isResting = false; // Usado solo para LookAndRest
    private float lookTimer = 0f; // Temporizador para LookAndRest

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Si hay waypoints, selecciona el primero
        if (waypoints.Length > 0)
            currentWaypoint = waypoints[0];
    }

    private void Update()
    {
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
        float rotationAngle = Mathf.PingPong(Time.time * rotationSpeed, 90f) - 45f; // Oscila entre -45 y 45 grados
        transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
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
        if (isResting)
        {
            lookTimer -= Time.deltaTime;
            if (lookTimer <= 0)
            {
                isResting = false;
                lookTimer = lookDuration;
            }
        }
        else
        {
            lookTimer -= Time.deltaTime;
            if (lookTimer <= 0)
            {
                isResting = true;
                lookTimer = restDuration;
            }

            // Rotación mientras está mirando
            float rotationAngle = Mathf.PingPong(Time.time * rotationSpeed, 90f) - 45f; // Oscila entre -45 y 45 grados
            transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
        }
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
                    
                case EnemyType.LookAndRest:
                    StartCoroutine(InvestigationLookAndRest());
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

    private IEnumerator InvestigationLookAndRest()
    {
        yield return new WaitForSeconds(lookDuration);
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

    // Dibuja waypoints en la vista del editor
    private void OnDrawGizmos()
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
    }
}
