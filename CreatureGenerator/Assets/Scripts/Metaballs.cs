using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Metaballs : MonoBehaviour {

    private struct Points
    {
        public Vector3 position;
        public bool inMeta;
    }

    private Mesh mesh;
    private List<Metaball> balls = new List<Metaball>();
    private Vector3 difference = new Vector3();
    private Vector3 startGrid;
    private List<Points> gridPoints = new List<Points>();
    private Vector2 gridItterations = new Vector2(); //x = z itterations, y = y itterations

    [SerializeField]
    private float excess = 2;
    [SerializeField]
    float detail = 1.0f;

    // Use this for initialization
    void Start () {
	}

    public void Generate(Creature c)
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "body";
        Vector3 max = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 min = new Vector3(0.0f, 0.0f, 0.0f);

        for(int i = 0; i < c.points.Count; i++)
        {
            balls.Add(new Metaball(c.points[i]));
            max = Max(c.points[i], max);
            min = Min(c.points[i], min);
        }

        min = new Vector3(min.x - excess, min.y - excess, min.z - excess);
        max = new Vector3(max.x + excess, max.y + excess, max.z + excess);
        difference = max - min;
        startGrid = min;
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(float x = startGrid.x; x < startGrid.x + difference.x+detail; x += detail)
        {
            for(float y = startGrid.y; y < startGrid.y + difference.y + detail; y += detail)
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
                    Points p = new Points
                    {
                        position = new Vector3(x, y, z),
                        inMeta = false
                    };
                    int index = gridPoints.Count - 1;

                    for (int i = 0; i < balls.Count; ++i)
                    {
                        Vector3 dir = balls[i].center - p.position;
                        if (dir.sqrMagnitude <= Mathf.Pow(balls[i].radius, 2))
                        {
                           p.inMeta =  true;
                        }
                    }
                    gridPoints.Add(p);
                }
            }
        }
        Debug.Log(gridItterations);
    }

    void GenerateMesh()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "creature bod";
        int move = (int)gridItterations.x * (int)gridItterations.y;
        for (int i = 0; i < gridPoints.Count - gridItterations.x * gridItterations.y; ++i)
        {
            if ((i/(int)gridItterations.x) % gridItterations.y == gridItterations.y - 1  || (i + 1) % gridItterations.x == 0)
            {
                continue;
            }
            Points[] point = new Points[8];
            point[0] = gridPoints[i];
            point[1] = gridPoints[i + 1];
            point[2] = gridPoints[i + (int)gridItterations.x];
            point[3] = gridPoints[i + (int)gridItterations.x + 1];
            point[4] = gridPoints[i + move];
            point[5] = gridPoints[i + move + 1];
            point[6] = gridPoints[i + move + (int)gridItterations.x];
            point[7] = gridPoints[i + move + (int)gridItterations.x + 1];

            List<int> theVertices = new List<int>();
            List<int> notIn = new List<int>();
            for (int j = 0; j < 8; ++j)
            {
                if (point[j].inMeta)
                {
                    theVertices.Add(j);
                }
                else
                {
                    notIn.Add(j);
                }
            }

            int count = theVertices.Count;
            if (count == 0 || count == 8)
            {
                continue;
            }

            switch (count)
            {
                case 1:
                    List<Vector3> triPos = new List<Vector3>();

                    if (theVertices[0] % 2 == 0)
                    {
                        Vector3 dir = point[theVertices[0] + 1].position - point[theVertices[0]].position;
                        float length = dir.magnitude/2;
                        dir = dir.normalized;
                        triPos.Add(dir * length);
                    }
                    else
                    {
                        Vector3 dir = point[theVertices[0] - 1].position - point[theVertices[0]].position;
                        float length = dir.magnitude / 2;
                        dir = dir.normalized;
                        triPos.Add(dir * length);
                    }
                    if (theVertices[0] % 4 == 0 || theVertices[0] == 3)
                    {
                        Vector3 dir = point[theVertices[0] + 2].position - point[theVertices[0]].position;
                        float length = dir.magnitude / 2;
                        dir = dir.normalized;
                        triPos.Add(dir * length);
                    }
                    else
                    {
                        Vector3 dir = point[theVertices[0] - 2].position - point[theVertices[0]].position;
                        float length = dir.magnitude / 2;
                        dir = dir.normalized;
                        triPos.Add(dir * length);
                    }
                    if (theVertices[0] < 4)
                    {
                        Vector3 dir = point[theVertices[0] + 4].position - point[theVertices[0]].position;
                        float length = dir.magnitude / 2;
                        dir = dir.normalized;
                        triPos.Add(dir * length);
                    }
                    else
                    {
                        Vector3 dir = point[theVertices[0] - 4].position - point[theVertices[0]].position;
                        float length = dir.magnitude / 2;
                        dir = dir.normalized;
                        triPos.Add(dir * length);
                    }
                    break;

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

    private void OnDrawGizmos()
    {
        if (gridPoints.Count == 0)
        {
            return;
        }
        //draw points of creature
        for (int i = 0; i < gridPoints.Count; i++)
        {
            if (gridPoints[i].inMeta)
            {
                Gizmos.color = Color.green;
            Gizmos.DrawSphere(gridPoints[i].position, 0.05f);
            }
            //else
            //{
            //    Gizmos.color = Color.black;
            //    Color temp = Gizmos.color;
            //    temp.a = 0.1f;
            //    Gizmos.color = temp;
            //}

        }
    }
}
