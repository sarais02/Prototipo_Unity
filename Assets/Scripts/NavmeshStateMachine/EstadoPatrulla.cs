using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EstadoPatrulla : Estado
{
    public NavMeshAgent agente;
    public List<Transform> waypoints = new List<Transform>();
    public int indiceActual;

    public EstadoPatrulla(NavMeshAgent agent, List<Transform> pos)
    {
        agente = agent;
        waypoints = pos;
    }

    override public void HacerAccion()
    {
        if (Vector3.Distance(agente.transform.position, waypoints[indiceActual].position) < 1)
        {
            indiceActual++;
            if(indiceActual >= waypoints.Count)
            {
                indiceActual = 0;
            }
        } else
        {
            agente.SetDestination(waypoints[indiceActual].position);
        }
    }
}
