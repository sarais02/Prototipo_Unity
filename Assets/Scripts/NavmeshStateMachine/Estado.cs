using UnityEngine;
using System.Collections.Generic;

public abstract class Estado
{
    public List<Transicion> transiciones = new List<Transicion>();

    public abstract void HacerAccion();
}
