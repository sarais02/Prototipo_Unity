using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Animator
    Animator animator;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float patrolSpeed;
    [SerializeField] private float investigationSpeed;
    [SerializeField] private float investigationTime;

    private Transform currentTarget;
    private bool isInvestigating = false;

    private void Start()
    {
        currentTarget = pointA;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isInvestigating)
        {
            Patrol();
            //Debug.Log("Patroling");
        }
    }

    private void Patrol()
    {
        Vector3 direction = (currentTarget.position - transform.position).normalized;
        transform.position += direction * patrolSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }

    public void Investigate(Vector3 distractionPosition) //Solo es public para llamarlo desde Distraction
    {
        if (!isInvestigating)
        {
            StartCoroutine(InvestigationRoutine(distractionPosition));
            animator.SetBool("Distraction", true);
        }
    }

    private IEnumerator InvestigationRoutine(Vector3 position)
    {
        isInvestigating = true;
        Debug.Log("Investing");

        while (Vector3.Distance(transform.position, position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, investigationSpeed * Time.deltaTime);
            /*
            Vector3 direction = (position - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }*/

            yield return null;
        }

        yield return new WaitForSeconds(investigationTime);

        isInvestigating = false;
        animator.SetBool("Distraction", false);
        currentTarget = (Vector3.Distance(transform.position, pointA.position) < Vector3.Distance(transform.position, pointB.position)) ? pointA : pointB;
    }

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }
}