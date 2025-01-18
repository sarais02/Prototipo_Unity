using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class MaquinaDeEstados : MonoBehaviour
{
    public Estado estadoActual;
    public NavMeshAgent agente;
    public List<Transform> waypoints = new List<Transform>();
    public GameObject player;

    private EstadoPatrulla patrulla;

    void Start()
    {
        List<Transform> listaJugador = new List<Transform>();
        listaJugador.Add(player.transform);

        patrulla = new EstadoPatrulla(agente, waypoints);
        EstadoPatrulla seguirJugador = new EstadoPatrulla(agente, listaJugador);

        CondicionCerca condicionCerca = new CondicionCerca(5, player.transform, transform);

        Transicion patrullaASeguirJugador = new Transicion(condicionCerca, seguirJugador);
        patrulla.transiciones.Add(patrullaASeguirJugador);

        estadoActual = patrulla;
    }

    void Update()
    {
        foreach(Transicion transicion in estadoActual.transiciones)
        {
            if (transicion.condicion.Comprobar())
            {
                estadoActual = transicion.siguienteEstado;
            }
        }
        estadoActual.HacerAccion();
    }

    public void ComprobarPan(Transform pan)
    {
        List<Transform> listaDelPan = new List<Transform>();
        listaDelPan.Add(pan);
        EstadoPatrulla seguirPan = new EstadoPatrulla(agente, listaDelPan);
        CondicionCerca condicionCercaPan = new CondicionCerca(10, pan, transform);
        Transicion patrullaASeguirPan = new Transicion(condicionCercaPan, seguirPan);
        patrulla.transiciones.Add(patrullaASeguirPan);
        estadoActual = patrulla;
    }

    public void VolverAPatrullar()
    {
        List<Transform> listaJugador = new List<Transform>();
        listaJugador.Add(player.transform);

        patrulla = new EstadoPatrulla(agente, waypoints);
        EstadoPatrulla seguirJugador = new EstadoPatrulla(agente, listaJugador);

        CondicionCerca condicionCerca = new CondicionCerca(5, player.transform, transform);

        Transicion patrullaASeguirJugador = new Transicion(condicionCerca, seguirJugador);
        patrulla.transiciones.Add(patrullaASeguirJugador);

        estadoActual = patrulla;
    }

    private void Asesinato()
    {
        player.GetComponent<PlayerRespawn>()?.KillPlayer(0f);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == player) Asesinato();
    }
}
