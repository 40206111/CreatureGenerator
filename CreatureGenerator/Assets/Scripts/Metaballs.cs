using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class Metaballs : MonoBehaviour {

    private Mesh mesh;
    private List<Metaball> balls = new List<Metaball>();
    private Vector3 difference = new Vector3();
    private Vector3 startGrid;
    private List<Vector3> gridPoints = new List<Vector3>();

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
                for(float z = startGrid.z; z < startGrid.z + difference.z + detail; z += detail)
                {
                    gridPoints.Add(new Vector3(x, y, z));
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

    private void OnDrawGizmos()
    {
        if (gridPoints.Count == 0)
        {
            return;
        }
        Gizmos.color = Color.red;
        Color temp = Gizmos.color;
        temp.a = 0.2f;
        Gizmos.color = temp;
        Gizmos.DrawSphere(gridPoints[0], 0.1f);

        Gizmos.color = Color.black;
        temp = Gizmos.color;
        temp.a = 0.2f;
        Gizmos.color = temp;
        //draw points of creature
        for (int i = 1; i < gridPoints.Count; i++)
        {
            Gizmos.DrawSphere(gridPoints[i], 0.1f);
        }
    }
}
