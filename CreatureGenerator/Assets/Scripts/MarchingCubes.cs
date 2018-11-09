using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes
{
    public struct Points
    {
        public Vector3 position;
        public bool inMeta;
    }

    //Array of ajacent indices in clockwise order
    private static readonly int[][] adjIndices = new int[][]{
        new int[]{1,2,4 }, //0
        new int[]{5,3,0 }, //1
        new int[]{6,0,3 }, //2
        new int[]{2,1,7 }, //3
        new int[]{0,6,5 }, //4
        new int[]{4,7,1 }, //5
        new int[]{7,4,2 }, //6
        new int[]{3,5,6 }  //7
    };

    //Method for generating mesh
    public static void GenerateMesh(List<Points> gridPoints, Vector2 gridItterations, ref Mesh mesh)
    {
        //how much to move by to find next point on x
        int move = (int)gridItterations.x * (int)gridItterations.y;
        //initialise lists
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        //Loop through grid points excluding the last section
        for (int i = 0; i < gridPoints.Count - gridItterations.x * gridItterations.y; ++i)
        {
            //exclude the edge sections
            if ((i / (int)gridItterations.x) % gridItterations.y == gridItterations.y - 1 || (i + 1) % gridItterations.x == 0)
            {
                continue;
            }

            //Calculate cube points
            Points[] point = new Points[8];
            point[0] = gridPoints[i];
            point[1] = gridPoints[i + 1];
            point[2] = gridPoints[i + (int)gridItterations.x];
            point[3] = gridPoints[i + (int)gridItterations.x + 1];
            point[4] = gridPoints[i + move];
            point[5] = gridPoints[i + move + 1];
            point[6] = gridPoints[i + move + (int)gridItterations.x];
            point[7] = gridPoints[i + move + (int)gridItterations.x + 1];

            //Find vertices within and outwith metaball
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

            //Do condition based on amount of vertices in metaball
            int count = theVertices.Count;
            switch (count)
            {
                case 1:
                    OnePoint(point, theVertices[0], ref verts, ref tris, true);
                    break;
                case 2:
                    // if vertices are ajacent
                    if (adjIndices[theVertices[0]][0] == theVertices[1] ||
                        adjIndices[theVertices[0]][1] == theVertices[1] ||
                        adjIndices[theVertices[0]][2] == theVertices[1])
                    {
                        Ajacent(point, new Vector2Int(theVertices[0], theVertices[1]), ref verts, ref tris, true);
                    }
                    else
                    {
                        OnePoint(point, theVertices[0], ref verts, ref tris, true);
                        OnePoint(point, theVertices[1], ref verts, ref tris, true);
                    }
                    break;
                case 3:
                    // if all vertices are connected
                    if (Connected(theVertices, point)[theVertices[0]].Count == 3)
                    {
                        ThreeTog(point, theVertices, ref verts, ref tris, true);
                    }
                    else if (adjIndices[theVertices[0]][0] == theVertices[1] ||
                        adjIndices[theVertices[0]][1] == theVertices[1] ||
                        adjIndices[theVertices[0]][2] == theVertices[1])   //0 and 1 are ajacent
                    {
                        Ajacent(point, new Vector2Int(theVertices[0], theVertices[1]), ref verts, ref tris, true);
                        OnePoint(point, theVertices[2], ref verts, ref tris, true);
                    }
                    else if (adjIndices[theVertices[0]][0] == theVertices[2] ||
                             adjIndices[theVertices[0]][1] == theVertices[2] ||
                             adjIndices[theVertices[0]][2] == theVertices[2])   //0 and 2 are ajacent
                    {
                        Ajacent(point, new Vector2Int(theVertices[0], theVertices[2]), ref verts, ref tris, true);
                        OnePoint(point, theVertices[1], ref verts, ref tris, true);
                    }
                    else if (adjIndices[theVertices[1]][0] == theVertices[2] ||
                             adjIndices[theVertices[1]][1] == theVertices[2] ||
                             adjIndices[theVertices[1]][2] == theVertices[2])   //1 and 2 are adjacent
                    {
                        Ajacent(point, new Vector2Int(theVertices[1], theVertices[2]), ref verts, ref tris, true);
                        OnePoint(point, theVertices[0], ref verts, ref tris, true);
                    }
                    else
                    {
                        OnePoint(point, theVertices[0], ref verts, ref tris, true);
                        OnePoint(point, theVertices[1], ref verts, ref tris, true);
                        OnePoint(point, theVertices[2], ref verts, ref tris, true);
                    }
                    break;
                case 4:
                    //Get all connected vertices
                    Dictionary<int, List<int>> connected = Connected(theVertices, point);

                    //check how many seperate sections there are
                    switch (connected.Count)
                    {
                        case 1:
                            if (Loop(theVertices, point)) //if the vetices cycle
                            {
                                Plane(point, theVertices, ref verts, ref tris);
                            }
                            else
                            {
                                //find if there is a point in the middle
                                int start = 0;
                                int adj = 0;
                                bool middle = false;

                                foreach (int v in theVertices)
                                {
                                    for (int j = 0; j < 3; j++)
                                    {
                                        if (point[adjIndices[v][j]].inMeta)
                                        {
                                            ++adj;
                                        }
                                    }
                                    if (adj == 1)
                                    {
                                        start = v;
                                    }
                                    if (adj == 3)
                                    {
                                        start = v;
                                        middle = true;
                                        break;
                                    }
                                    adj = 0;
                                }

                                if (middle)
                                {
                                    MidFour(start, point, theVertices, ref verts, ref tris);
                                }
                                else
                                {
                                    ChainFour(start, point, theVertices, ref verts, ref tris);
                                }
                            }
                            break;
                        case 2:
                            if (connected[theVertices[0]].Count == 2)
                            {
                                //for both adjacent pairs
                                foreach (KeyValuePair<int, List<int>> c in connected)
                                {
                                    Ajacent(point, new Vector2Int(c.Key, c.Value[0]), ref verts, ref tris, true);
                                }
                            }
                            else
                            {
                                //find which point is alone
                                if (connected[theVertices[0]].Count == 1)
                                {
                                    foreach (KeyValuePair<int, List<int>> c in connected)
                                    {
                                        if (c.Key == theVertices[0])
                                        {
                                            OnePoint(point, c.Value[0], ref verts, ref tris, true);

                                        }
                                        else
                                        {
                                            ThreeTog(point, c.Value, ref verts, ref tris, true);
                                        }
                                    }
                                }
                                else if (connected[theVertices[0]].Count == 3)
                                {
                                    foreach (KeyValuePair<int, List<int>> c in connected)
                                    {
                                        if (c.Key == theVertices[0])
                                        {
                                            ThreeTog(point, c.Value, ref verts, ref tris, true);
                                        }
                                        else
                                        {
                                            OnePoint(point, c.Value[0], ref verts, ref tris, true);
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            OnePoint(point, theVertices[0], ref verts, ref tris, true);
                            OnePoint(point, theVertices[1], ref verts, ref tris, true);
                            OnePoint(point, theVertices[2], ref verts, ref tris, true);
                            OnePoint(point, theVertices[3], ref verts, ref tris, true);
                            break;
                    }

                    break;
                case 5:
                    if (Connected(notIn, point)[notIn[0]].Count == 3) //if three connected points not in the meta ball
                    {
                        ThreeTog(point, notIn, ref verts, ref tris, false);
                    }
                    else if (adjIndices[notIn[0]][0] == notIn[1] ||
                        adjIndices[notIn[0]][1] == notIn[1] ||
                        adjIndices[notIn[0]][2] == notIn[1])    // if 0 and 1 are adjacent and not in the meta ball
                    {
                        Ajacent(point, new Vector2Int(notIn[0], notIn[1]), ref verts, ref tris, false);
                        OnePoint(point, notIn[2], ref verts, ref tris, false);
                    }
                    else if (adjIndices[notIn[0]][0] == notIn[2] ||
                             adjIndices[notIn[0]][1] == notIn[2] ||
                             adjIndices[notIn[0]][2] == notIn[2])   // if 0 and 2 are adjacent and not in the meta ball
                    {
                        Ajacent(point, new Vector2Int(notIn[0], notIn[2]), ref verts, ref tris, false); 
                        OnePoint(point, notIn[1], ref verts, ref tris, false);
                    }
                    else if (adjIndices[notIn[1]][0] == notIn[2] ||
                             adjIndices[notIn[1]][1] == notIn[2] ||
                             adjIndices[notIn[1]][2] == notIn[2])   // if 1 and 2 are adjacent and not in the meta ball
                    {
                        Ajacent(point, new Vector2Int(notIn[1], notIn[2]), ref verts, ref tris, false);
                        OnePoint(point, notIn[0], ref verts, ref tris, false);
                    }
                    else
                    {
                        OnePoint(point, notIn[0], ref verts, ref tris, false);
                        OnePoint(point, notIn[1], ref verts, ref tris, false);
                        OnePoint(point, notIn[2], ref verts, ref tris, false);
                    }
                    break;
                case 6:
                    //if vertices not in meta ball are adjacent
                    if (adjIndices[notIn[0]][0] == notIn[1] ||
                        adjIndices[notIn[0]][1] == notIn[1] ||
                        adjIndices[notIn[0]][2] == notIn[1])
                    {
                        Ajacent(point, new Vector2Int(notIn[0], notIn[1]), ref verts, ref tris, false);
                    }
                    else
                    {
                        OnePoint(point, notIn[0], ref verts, ref tris, false);
                        OnePoint(point, notIn[1], ref verts, ref tris, false);
                    }
                    break;
                case 7:
                    OnePoint(point, notIn[0], ref verts, ref tris, false);
                    break;
                default:
                    break;

            }
        }
        //set mesh vertices and triangles
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
    }

    //////////Geomtry Functions///////////
    //Method to add geometry if there is only one point
    private static void OnePoint(Points[] point, int thePoint, ref List<Vector3> verts, ref List<int> tris, bool reverse)
    {
        int start = 0;
        int end = 3;
        int increment = 1;
        if (reverse)
        {
            start = 2;
            end = -1;
            increment = -1;
        }
        
        //loop through adjacent point in direction depending if is reversed
        for (int i = start; i != end; i += increment)
        {
            Vector3 dir = point[adjIndices[thePoint][i]].position - point[thePoint].position;
            dir = point[thePoint].position + dir / 2;
            //Only add dir to verts if it doesn't already exsist
            if (verts.Contains(dir))
            {
                tris.Add(verts.IndexOf(dir));
            }
            else
            {
                verts.Add(dir);
                tris.Add(verts.Count - 1);
            }
        }
    }

    //Method for generating mesh if 2 points are adjacent
    private static void Ajacent(Points[] point, Vector2Int thePoints, ref List<Vector3> verts, ref List<int> tris, bool reverse)
    {
        int start1 = 0;
        int start2 = 0;
        List<int> temTris = new List<int>();

        //find adjacent points across from given points
        for (int i = 0; i < 3; ++i)
        {
            if (adjIndices[thePoints[0]][i] == thePoints[1])
            {
                start1 = i;
            }
            if (adjIndices[thePoints[1]][i] == thePoints[0])
            {
                start2 = i;
            }
        }

        //Add first 2 verts
        for (int i = start1 + 1; i < start1 + 3; ++i)
        {
            Vector3 dir = point[adjIndices[thePoints[0]][i % 3]].position - point[thePoints[0]].position;
            dir = point[thePoints[0]].position + dir / 2;
            //Only add dir if it's not already in verts
            if (verts.Contains(dir))
            {
                temTris.Add(verts.IndexOf(dir));
            }
            else
            {
                verts.Add(dir);
                temTris.Add(verts.Count - 1);
            }
        }
        //Add second verts
        for (int i = start2 + 1; i < start2 + 3; ++i)
        {
            Vector3 dir = point[adjIndices[thePoints[1]][i % 3]].position - point[thePoints[1]].position;
            dir = point[thePoints[1]].position + dir / 2;
            //Only add dir if it's not already in verts
            if (verts.Contains(dir))
            {
                temTris.Add(verts.IndexOf(dir));
            }
            else
            {
                verts.Add(dir);
                temTris.Add(verts.Count - 1);
            }
        }

        //add triangles
        if (reverse)
        {
            tris.Add(temTris[temTris.Count - 1]);
            tris.Add(temTris[temTris.Count - 3]);
            tris.Add(temTris[temTris.Count - 4]);
            tris.Add(temTris[temTris.Count - 1]);
            tris.Add(temTris[temTris.Count - 2]);
            tris.Add(temTris[temTris.Count - 3]);
        }
        else
        {
            tris.Add(temTris[temTris.Count - 4]);
            tris.Add(temTris[temTris.Count - 3]);
            tris.Add(temTris[temTris.Count - 1]);
            tris.Add(temTris[temTris.Count - 3]);
            tris.Add(temTris[temTris.Count - 2]);
            tris.Add(temTris[temTris.Count - 1]);
        }
    }
    
    //Method for generating geometry when 3 points are connected
    private static void ThreeTog(Points[] point, List<int> thePoints, ref List<Vector3> verts, ref List<int> tris, bool reverse)
    {
        int test = 1;
        int middle = 2;
        bool once = false;

        //find middle
        for (int i = 0; i < 2; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                if (adjIndices[thePoints[i]][j] == thePoints[2] ||
                    adjIndices[thePoints[i]][j] == thePoints[test])
                {
                    if (once)
                    {
                        middle = i;
                    }
                    once = true;
                }
            }
            once = false;
            test--;
        }
        List<int> orderedPoints = new List<int>
        {
            thePoints[middle]
        };
        int second = 0;
        int third = 0;
        bool change = true;

        //order points
        for (int i = 0; i < 3; ++i)
        {
            if (once && point[adjIndices[thePoints[middle]][i]].inMeta == point[thePoints[middle]].inMeta)
            {
                third = adjIndices[thePoints[middle]][i];
                break;
            }
            else if (once)
            {
                change = false;
            }
            else if (point[adjIndices[thePoints[middle]][i]].inMeta == point[thePoints[middle]].inMeta)
            {
                second = adjIndices[thePoints[middle]][i];
                once = true;
            }
        }

        if (change)
        {
            orderedPoints.Add(third);
            orderedPoints.Add(second);
        }
        else
        {
            orderedPoints.Add(second);
            orderedPoints.Add(third);
        }

        //Calculate vertices positions
        List<int> positions = AddPointsClockwise(orderedPoints, point, ref verts);

        //add triangles
        if (reverse)
        {
            tris.Add(positions[2]);
            tris.Add(positions[3]);
            tris.Add(positions[0]);
            tris.Add(positions[3]);
            tris.Add(positions[2]);
            tris.Add(positions[1]);
            tris.Add(positions[4]);
            tris.Add(positions[3]);
            tris.Add(positions[1]);
        }
        else
        {
            tris.Add(positions[0]);
            tris.Add(positions[3]);
            tris.Add(positions[2]);
            tris.Add(positions[1]);
            tris.Add(positions[2]);
            tris.Add(positions[3]);
            tris.Add(positions[1]);
            tris.Add(positions[3]);
            tris.Add(positions[4]);
        }


    }

    //Method to generate a plane when given for connected points that cycle
    private static void Plane(Points[] point, List<int> thePoints, ref List<Vector3> verts, ref List<int> tris)
    {
        //initialise variables
        int start = thePoints[0];
        int prev = thePoints[0];
        int current = thePoints[0];
        List<int> orderedList = new List<int>
        {
            start
        };
        bool reverse = true;

        //order list
        for (int j = 0; j < 3; ++j)
        {
            if (adjIndices[current][j] != prev &&
                point[adjIndices[current][j]].inMeta == point[current].inMeta)
            {
                prev = current;
                current = adjIndices[current][j];
                //reverse geometry if necisarry
                if ((point[current].position - point[prev].position).normalized.y != 0)
                {
                    reverse = false;
                }
                j = -1;
                if (current == start)
                {
                    break;
                }
                orderedList.Add(current);
            }
        }

        //Calculate vertices positions
        List<int> positions = AddPoints(orderedList, point, ref verts);

        //Add triangles
        if (reverse)
        {
            tris.Add(positions[0]);
            tris.Add(positions[1]);
            tris.Add(positions[3]);
            tris.Add(positions[1]);
            tris.Add(positions[2]);
            tris.Add(positions[3]);
        }
        else
        {
            tris.Add(positions[3]);
            tris.Add(positions[1]);
            tris.Add(positions[0]);
            tris.Add(positions[3]);
            tris.Add(positions[2]);
            tris.Add(positions[1]);
        }

    }

    //Method for if there are 4 connected with a point in the middle
    private static void MidFour(int middle, Points[] point, List<int> thePoints, ref List<Vector3> verts, ref List<int> tris)
    {
        List<int> ordered = new List<int>
        {
            middle
        };

        //order inces
        for (int i = 0; i < 3; i++)
        {
            ordered.Add(adjIndices[middle][i]);
        }

        //Find Vertices
        List<int> positions = AddPoints(ordered, point, ref verts);

        //Add triangles
        tris.Add(positions[5]);
        tris.Add(positions[4]);
        tris.Add(positions[0]);
        tris.Add(positions[4]);
        tris.Add(positions[1]);
        tris.Add(positions[0]);
        tris.Add(positions[4]);
        tris.Add(positions[2]);
        tris.Add(positions[1]);
        tris.Add(positions[2]);
        tris.Add(positions[3]);
        tris.Add(positions[1]);
    }

    //Method for if 4 points are connect that do not have a point in the middle
    private static void ChainFour(int start, Points[] point, List<int> thePoints, ref List<Vector3> verts, ref List<int> tris)
    {
        List<int> ordered = new List<int>
        {
            start
        };

        //order indices
        for (int i = 0; i < 3; i++)
        {
            if (!ordered.Contains(adjIndices[start][i]) && point[adjIndices[start][i]].inMeta)
            {
                start = adjIndices[start][i];
                ordered.Add(start);
                i = -1;
            }
        }

        bool change = true;
        bool once = false;

        // find if geometry needs to be reversed
        for (int i = 0; i < 3; ++i)
        {
            if (once && point[adjIndices[thePoints[start]][i]].inMeta == point[thePoints[start]].inMeta)
            {
                break;
            }
            else if (once)
            {
                change = false;
            }
            else if (point[adjIndices[thePoints[start]][i]].inMeta == point[thePoints[start]].inMeta)
            {
                once = true;
            }
        }

        //Calculate positions
        List<int> positions = AddPointsClockwise(ordered, point, ref verts);

        //Add triangles
        if (!change)
        {
            tris.Add(positions[3]);
            tris.Add(positions[5]);
            tris.Add(positions[4]);

            tris.Add(positions[1]);
            tris.Add(positions[3]);
            tris.Add(positions[4]);

            tris.Add(positions[2]);
            tris.Add(positions[1]);
            tris.Add(positions[4]);

            tris.Add(positions[1]);
            tris.Add(positions[0]);
            tris.Add(positions[3]);
        }
        else
        {
            tris.Add(positions[4]);
            tris.Add(positions[5]);
            tris.Add(positions[3]);

            tris.Add(positions[4]);
            tris.Add(positions[3]);
            tris.Add(positions[1]);

            tris.Add(positions[4]);
            tris.Add(positions[1]);
            tris.Add(positions[2]);

            tris.Add(positions[3]);
            tris.Add(positions[0]);
            tris.Add(positions[1]);
        }
    }

    //////////Helper Functions///////////
    //Add points in clockwise order
    private static List<int>  AddPointsClockwise(List<int> theVertices, Points[] point, ref List<Vector3> verts)
    {
        //initialise variables
        Points point1;
        Points point2;
        List<int> output = new List<int>();
        bool go = false;
        int startInt = 0;

        //loop through vertices
        for (int i = 0; i < theVertices.Count; ++i)
        {
            point1 = point[theVertices[i]];
            //loop through adjacences
            for (int j = 0; j < 3; ++j)
            {
                point2 = point[adjIndices[theVertices[i]][(startInt + j) % 3]];

                //if points connected
                if (point1.inMeta != point2.inMeta)
                {
                    if (go) //if at the right point in list
                    {
                        //find direction
                        Vector3 dir = point2.position - point1.position;
                        dir = point1.position + dir / 2;
                        //Add direction if it doesn't already exsist
                        if (verts.Contains(dir))
                        {
                            output.Add(verts.IndexOf(dir));
                        }
                        else
                        {
                            verts.Add(dir);
                            output.Add(verts.Count - 1);
                        }
                    }
                }
                else if (!go)
                {
                    go = true;
                    startInt = j;
                    j = -1;
                }
            }
            startInt = 0;
            go = false;
        }

        return output;
    }

    //Method to add points to vertices
    private static List<int> AddPoints(List<int> theVertices, Points[] point, ref List<Vector3> verts)
    {
        Points point1;
        Points point2;
        List<int> output = new List<int>();

        //Loop through given vertices
        for (int i = 0; i < theVertices.Count; ++i)
        {
            point1 = point[theVertices[i]];
            for (int j = 0; j < 3; ++j)
            {
                point2 = point[adjIndices[theVertices[i]][j]];
                //if the points are not both in or out the metaball
                if (point1.inMeta != point2.inMeta)
                {
                    //calculate direction
                    Vector3 dir = point2.position - point1.position;
                    dir = point1.position + dir / 2;
                    //only add direction to verts if it doesn't already exsist
                    if (verts.Contains(dir))
                    {
                        output.Add(verts.IndexOf(dir));
                    }
                    else
                    {
                        verts.Add(dir);
                        output.Add(verts.Count - 1);
                    }
                }
            }
        }

        return output;
    }

    //Method to help connect find how many points are conected
    private static List<int> ConnectHelp(int start, ref List<int> next, Points[] point, ref List<int> tested)
    {
        List<int> output = new List<int>();
        bool meta = point[start].inMeta;
        //remove start from next
        next.Remove(start);

        for (int i = 0; i < 3; ++i)
        {
            //if not already tested and connected
            if (!tested.Contains(adjIndices[start][i]) && point[adjIndices[start][i]].inMeta == meta)
            {
                tested.Add(adjIndices[start][i]);
                //check for future connected points
                output.AddRange(ConnectHelp(adjIndices[start][i], ref next, point, ref tested));
                output.Add(adjIndices[start][i]);
            }
        }
        return output;

    }

    //Method to check how many points are connected
    private static Dictionary<int, List<int>> Connected(List<int> theVertices, Points[] point)
    {
        Dictionary<int, List<int>> connects = new Dictionary<int, List<int>>();
        List<int> next = new List<int>(theVertices);
        List<int> tested = new List<int>();
        List<int> connectors = new List<int>();
        int start;
        
        //while there are still unchecked given vertices
        while (next.Count > 0)
        {
            start = next[0];
            tested.Add(start);
            connectors.Add(start);
            connectors.AddRange(ConnectHelp(start, ref next, point, ref tested));
            connects.Add(start, new List<int>(connectors));
            tested.Clear();
            connectors.Clear();
        }

        return connects;
    }

    //Method to check if vertices loop
    private static bool Loop(List<int> theVertices, Points[] point)
    {
        int start = theVertices[0];
        int prev = theVertices[0];
        int current = theVertices[0];
        bool loop = false;

        for (int j = 0; j < 3; ++j)
        {
            // if adjacent isn't the last one checked and is connected to current
            if (adjIndices[current][j] != prev &&
                point[adjIndices[current][j]].inMeta == point[current].inMeta)
            {
                prev = current;
                current = adjIndices[current][j];
                j = -1;
                if (current == start) //if current is the same as start the vertices loop
                {
                    loop = true;
                    break;
                }
            }
        }

        return loop;
    }


}
