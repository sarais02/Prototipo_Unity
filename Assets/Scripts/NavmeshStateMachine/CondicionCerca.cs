using UnityEngine;

public class CondicionCerca : Condicion
{
    public float distancia;
    public Transform objetivo, transform;

    public CondicionCerca(float distancia, Transform objetivo, Transform transform)
    {
        this.distancia = distancia;
        this.transform = transform;
        this.objetivo = objetivo;
    }

    public override bool Comprobar()
    {
        return Vector3.Distance(objetivo.position, transform.position) < distancia;
    }
}
