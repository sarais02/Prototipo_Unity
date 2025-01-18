using UnityEngine;

public class Transicion
{
    public Condicion condicion;
    public Estado siguienteEstado;

    public Transicion (Condicion con, Estado est)
    {
        condicion = con;
        siguienteEstado = est;
    }
}
