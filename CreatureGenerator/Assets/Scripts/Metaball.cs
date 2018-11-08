using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metaball
{
    public Vector3 center;
    public float radius = 0.2f;
    public float spread = 0.2f;

    public Metaball(Vector3 c)
    {
        center = c;
    }
}
