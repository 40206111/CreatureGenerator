using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Metaballs : MonoBehaviour
{

    private Mesh mesh;
    private List<Metaball> balls = new List<Metaball>();
    private Vector3 difference = new Vector3();
    private Vector3 startGrid;
    private List<MarchingCubes.Points> gridPoints = new List<MarchingCubes.Points>();
    private Vector2 gridItterations = new Vector2(); //x = z itterations, y = y itterations

    [SerializeField]
    private float excess = 2;
    [SerializeField]
    float detail = 1.0f;

    private void MakeBalls(Creature c)
    {
        balls.Add(new Metaball(c.Start, 0.4f, 0.5f));
        foreach (List<Vector3> l in c.Points["Torso"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.5f, 0.5f));
            }
        }
        foreach (List<Vector3> l in c.Points["Head"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.4f, 0.2f));
            }
        }
        foreach (List<Vector3> l in c.Points["Neck"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.1f, 0.2f));
            }
        }
        foreach (List<Vector3> l in c.Points["Arm"])
        {
            for (int i = 0; i < l.Count; ++i)
            {
                if (i != 0)
                {
                    balls.Add(new Metaball(l[i] + ((l[i - 1] - l[i]) / 2), 0.2f, 0.5f));

                }
                balls.Add(new Metaball(l[i], 0.2f, 0.5f));
            }
        }
        foreach (List<Vector3> l in c.Points["Leg"])
        {
            for (int i = 0; i < l.Count; ++i)
            {
                if (i != 0)
                {
                    balls.Add(new Metaball(l[i] + ((l[i - 1] - l[i]) / 2), 0.3f, 0.3f));

                }
                balls.Add(new Metaball(l[i], 0.3f, 0.3f));
            }
        }
        foreach (List<Vector3> l in c.Points["Spine"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.4f, 1.2f));
            }
        }

        foreach (List<Vector3> l in c.Points["Tail"])
        {
            for (int i = 0; i < l.Count; ++i)
            {
                if (i != 0)
                {
                    balls.Add(new Metaball(l[i] + ((l[i-1] - l[i])/2), 0.15f, 0.4f));

                }
                balls.Add(new Metaball(l[i], 0.15f, 0.4f));
            }
        }

    }

    public void Generate(Creature c)
    {
        Vector3 max = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 min = new Vector3(0.0f, 0.0f, 0.0f);

        MakeBalls(c);

        min = new Vector3(min.x - excess, min.y - excess, min.z - excess);
        max = new Vector3(max.x + excess, max.y + excess, max.z + excess);
        difference = max - min;
        startGrid = min;
        GenerateGrid();
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "creature bod";
        MarchingCubes.GenerateMesh(gridPoints, gridItterations, ref mesh);
        mesh.RecalculateNormals();
    }

    void GenerateGrid()
    {
        for (float x = startGrid.x; x < startGrid.x + difference.x + detail; x += detail)
        {
            for (float y = startGrid.y; y < startGrid.y + difference.y + detail; y += detail)
            {
                if (x == startGrid.x)
                {
                    ++gridItterations.y;
                }
                for (float z = startGrid.z; z < startGrid.z + difference.z + detail; z += detail)
                {
                    if (x == startGrid.x && y == startGrid.y)
                    {
                        ++gridItterations.x;
                    }
                    MarchingCubes.Points p = new MarchingCubes.Points
                    {
                        position = new Vector3(x, y, z),
                        inMeta = false
                    };
                    float connected = 0;
                    for (int i = 0; i < balls.Count; ++i)
                    {
                        Vector3 dir = balls[i].center - p.position;
                        float length = dir.magnitude;

                        if (length <= (balls[i].radius + balls[i].spread))
                        {
                            float over = length - balls[i].radius;
                            if (over > 0)
                            {
                                connected += Mathf.Pow(1 - (over / balls[i].spread), 2);
                            }
                            else
                            {
                                connected += 1;
                            }
                        }
                        if (connected >= 1)
                        {
                            p.inMeta = true;
                        }
                    }
                    gridPoints.Add(p);
                }
            }
        }
    }

    Vector3 Max(Vector3 test, Vector3 against)
    {
        Vector3 output = against;
        if (test.x > against.x)
        {
            output.x = test.x;
        }
        if (test.y > against.y)
        {
            output.y = test.y;
        }
        if (test.z > against.z)
        {
            output.z = test.z;
        }
        return output;
    }

    Vector3 Min(Vector3 test, Vector3 against)
    {
        Vector3 output = against;
        if (test.x < against.x)
        {
            output.x = test.x;
        }
        if (test.y < against.y)
        {
            output.y = test.y;
        }
        if (test.z < against.z)
        {
            output.z = test.z;
        }
        return output;
    }
}
