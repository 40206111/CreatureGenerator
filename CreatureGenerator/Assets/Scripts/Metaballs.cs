using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Metaballs : MonoBehaviour
{
    private Mesh mesh;
    private List<Metaball> balls = new List<Metaball>();
    private Vector3 startGrid;
    private List<MarchingCubes.Points> gridPoints = new List<MarchingCubes.Points>();
    private Vector2 gridItterations = new Vector2(); //x = z itterations, y = y itterations
    private Vector3 max = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector3 min = new Vector3(0.0f, 0.0f, 0.0f);

    //Extra grid past last points of creature
    [SerializeField]
    private float excess = 2;
    //Amount of detail in creature
    [SerializeField]
    float detail = 1.0f;

    //Method to make meta balls
    private void MakeBalls(Creature c)
    {
        //Start point
        balls.Add(new Metaball(c.Start, 0.4f, 1.8f));

        ///TORSO///
        foreach (List<Vector3> l in c.Points["Torso"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.5f, 0.5f));
                max = Max(p, max);
                min = Min(p, min);
            }
        }

        ///HEAD///
        foreach (List<Vector3> l in c.Points["Head"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.4f, 0.2f));
                max = Max(p, max);
                min = Min(p, min);
            }
        }

        ///Neck///
        foreach (List<Vector3> l in c.Points["Neck"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.4f, 0.3f));
                max = Max(p, max);
                min = Min(p, min);
            }
        }

        ///ARM///
        foreach (List<Vector3> l in c.Points["Arm"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.2f, 0.5f));
                max = Max(p, max);
                min = Min(p, min);
            }
        }

        ///LEG///
        foreach (List<Vector3> l in c.Points["Leg"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.3f, 0.7f));
                max = Max(p, max);
                min = Min(p, min);
            }
        }

        ///SPINE///
        foreach (List<Vector3> l in c.Points["Spine"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.4f, 1.5f));
                max = Max(p, max);
                min = Min(p, min);
            }
        }

        ///TAIL///
        foreach (List<Vector3> l in c.Points["Tail"])
        {
            foreach (Vector3 p in l)
            {
                balls.Add(new Metaball(p, 0.2f, 0.2f));
                max = Max(p, max);
                min = Min(p, min);
            }
        }

    }

    //Method to generate given creature
    public void Generate(Creature c)
    {
        //Initialise variables
        balls.Clear();
        gridPoints.Clear();
        gridItterations = new Vector2();
        Vector3 difference = new Vector3();

        //Make meta balls
        MakeBalls(c);

        //Set max and min
        min = new Vector3(min.x - excess, min.y - excess, min.z - excess);
        max = new Vector3(max.x + excess, max.y + excess, max.z + excess);
        //Find difference of max and min
        difference = max - min;
        startGrid = min;
        
        //Make grid
        GenerateGrid(difference);

        //New mesh
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "creature bod";
        //Generate mesh
        MarchingCubes.GenerateMesh(gridPoints, gridItterations, ref mesh);
        //recalculate normals
        mesh.RecalculateNormals();
    }

    void GenerateGrid(Vector3 difference)
    {
        //loop through x points
        for (float x = startGrid.x; x < startGrid.x + difference.x + detail; x += detail)
        {
            //loop through y points
            for (float y = startGrid.y; y < startGrid.y + difference.y + detail; y += detail)
            {
                //calculate y itterations
                if (x == startGrid.x)
                {
                    ++gridItterations.y;
                }
                //loop through z points
                for (float z = startGrid.z; z < startGrid.z + difference.z + detail; z += detail)
                {
                    //calculate z itterations
                    if (x == startGrid.x && y == startGrid.y)
                    {
                        ++gridItterations.x;
                    }
                    
                    //point for grid
                    MarchingCubes.Points p = new MarchingCubes.Points
                    {
                        position = new Vector3(x, y, z),
                        inMeta = false
                    };

                    //calculate if verice is in metaball
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
                                //joins meta balls together
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
