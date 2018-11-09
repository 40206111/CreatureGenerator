using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metaball
{
    public Vector3 center;
    public float radius = 0.2f;
    public float spread = 1.3f;

    public Metaball(Vector3 c, float r, float s)
    {
        center = c;
        radius = r;
        spread = s;
    }
}
