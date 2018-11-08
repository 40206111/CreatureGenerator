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

    private static readonly int[][] adjIndices = new int[][]{
        new int[]{1,2,4 }, //0
        new int[]{5,3,0 }, //1
        new int[]{3,6,0 }, //2
        new int[]{7,2,1 }, //3
        new int[]{0,6,5 }, //4
        new int[]{4,7,1 }, //5
        new int[]{2,7,4 }, //6
        new int[]{6,3,5 }  //7
    };

    public static void GenerateMesh(List<Points> gridPoints, Vector2 gridItterations, ref Mesh mesh)
    {
        int move = (int)gridItterations.x * (int)gridItterations.y;
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        for (int i = 0; i < gridPoints.Count - gridItterations.x * gridItterations.y; ++i)
        {
            if ((i / (int)gridItterations.x) % gridItterations.y == gridItterations.y - 1 || (i + 1) % gridItterations.x == 0)
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
            switch (count)
            {
                case 1:
                    onePoint(point, theVertices[0], ref verts, ref tris, false);
                    break;
                case 2:
                    if (adjIndices[theVertices[0]][0] == theVertices[1] ||
                        adjIndices[theVertices[0]][1] == theVertices[1] ||
                        adjIndices[theVertices[0]][2] == theVertices[1])
                    {
                        ajacent(point, new Vector2Int(theVertices[0], theVertices[1]), ref verts, ref tris, false);
                    }
                    else
                    {
                        onePoint(point, theVertices[0], ref verts, ref tris, false);
                        onePoint(point, theVertices[1], ref verts, ref tris, false);
                    }
                    break;
                case 3:
                    if (Connected(theVertices, point)[theVertices[0]].Count == 3)
                    {
                        ThreeTog(point, theVertices, ref verts, ref tris, false);
                    }
                    else if (adjIndices[theVertices[0]][0] == theVertices[1] ||
                        adjIndices[theVertices[0]][1] == theVertices[1] ||
                        adjIndices[theVertices[0]][2] == theVertices[1])
                    {
                        ajacent(point, new Vector2Int(theVertices[0], theVertices[1]), ref verts, ref tris, false);
                        onePoint(point, theVertices[2], ref verts, ref tris, false);
                    }
                    else if (adjIndices[theVertices[0]][0] == theVertices[2] ||
                             adjIndices[theVertices[0]][1] == theVertices[2] ||
                             adjIndices[theVertices[0]][2] == theVertices[2])
                    {
                        ajacent(point, new Vector2Int(theVertices[0], theVertices[2]), ref verts, ref tris, false);
                        onePoint(point, theVertices[1], ref verts, ref tris, false);
                    }
                    else if (adjIndices[theVertices[1]][0] == theVertices[2] ||
                             adjIndices[theVertices[1]][1] == theVertices[2] ||
                             adjIndices[theVertices[1]][2] == theVertices[2])
                    {
                        ajacent(point, new Vector2Int(theVertices[1], theVertices[2]), ref verts, ref tris, false);
                        onePoint(point, theVertices[0], ref verts, ref tris, false);
                    }
                    else
                    {
                        onePoint(point, theVertices[0], ref verts, ref tris, false);
                        onePoint(point, theVertices[1], ref verts, ref tris, false);
                        onePoint(point, theVertices[2], ref verts, ref tris, false);
                    }
                    break;
                case 4:
                    Dictionary<int, List<int>> connected = Connected(theVertices, point);

                    switch (connected.Count)
                    {
                        case 1:
                            if (Loop(theVertices, point))
                            {
                                Debug.Log("test");
                                Plane(point, theVertices, ref verts, ref tris, false);
                            }
                            break;
                        case 2:
                            if (connected[theVertices[0]].Count == 2)
                            {
                                foreach (KeyValuePair<int, List<int>> c in connected)
                                {
                                    ajacent(point, new Vector2Int(c.Key, c.Value[0]), ref verts, ref tris, false);
                                }
                            }
                            else
                            {
                                if (connected[theVertices[0]].Count == 1)
                                {
                                    foreach (KeyValuePair<int, List<int>> c in connected)
                                    {
                                        if (c.Key == theVertices[0])
                                        {
                                            onePoint(point, c.Value[0], ref verts, ref tris, false);

                                        }
                                        else
                                        {
                                            ThreeTog(point, c.Value, ref verts, ref tris, false);
                                        }
                                    }
                                }
                                else if (connected[theVertices[0]].Count == 3)
                                {
                                    foreach (KeyValuePair<int, List<int>> c in connected)
                                    {
                                        if (c.Key == theVertices[0])
                                        {
                                            ThreeTog(point, c.Value, ref verts, ref tris, false);
                                        }
                                        else
                                        {
                                            onePoint(point, c.Value[0], ref verts, ref tris, false);
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            onePoint(point, theVertices[0], ref verts, ref tris, false);
                            onePoint(point, theVertices[1], ref verts, ref tris, false);
                            onePoint(point, theVertices[2], ref verts, ref tris, false);
                            onePoint(point, theVertices[3], ref verts, ref tris, false);
                            break;
                    }

                    break;
                case 5:
                    if (Connected(notIn, point)[notIn[0]].Count == 2)
                    {
                        ThreeTog(point, notIn, ref verts, ref tris, true);
                    }
                    else if (adjIndices[notIn[0]][0] == notIn[1] ||
                        adjIndices[notIn[0]][1] == notIn[1] ||
                        adjIndices[notIn[0]][2] == notIn[1])
                    {
                        ajacent(point, new Vector2Int(notIn[0], notIn[1]), ref verts, ref tris, true);
                        onePoint(point, notIn[2], ref verts, ref tris, true);
                    }
                    else if (adjIndices[notIn[0]][0] == notIn[2] ||
                             adjIndices[notIn[0]][1] == notIn[2] ||
                             adjIndices[notIn[0]][2] == notIn[2])
                    {
                        ajacent(point, new Vector2Int(notIn[0], notIn[2]), ref verts, ref tris, true);
                        onePoint(point, notIn[1], ref verts, ref tris, true);
                    }
                    else if (adjIndices[notIn[1]][0] == notIn[2] ||
                             adjIndices[notIn[1]][1] == notIn[2] ||
                             adjIndices[notIn[1]][2] == notIn[2])
                    {
                        ajacent(point, new Vector2Int(notIn[1], notIn[2]), ref verts, ref tris, true);
                        onePoint(point, notIn[0], ref verts, ref tris, true);
                    }
                    else
                    {
                        onePoint(point, notIn[0], ref verts, ref tris, true);
                        onePoint(point, notIn[1], ref verts, ref tris, true);
                        onePoint(point, notIn[2], ref verts, ref tris, true);
                    }
                    break;
                case 6:
                    if (adjIndices[notIn[0]][0] == notIn[1] ||
                        adjIndices[notIn[0]][1] == notIn[1] ||
                        adjIndices[notIn[0]][2] == notIn[1])
                    {
                        ajacent(point, new Vector2Int(notIn[0], notIn[1]), ref verts, ref tris, true);
                    }
                    else
                    {
                        onePoint(point, notIn[0], ref verts, ref tris, true);
                        onePoint(point, notIn[1], ref verts, ref tris, true);
                    }
                    break;
                case 7:
                    onePoint(point, notIn[0], ref verts, ref tris, true);
                    break;
                default:
                    break;

            }
        }
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
    }

    private static void Plane(Points[] point, List<int> thePoints, ref List<Vector3> verts, ref List<int> tris, bool reverse)
    {
        int start = thePoints[0];
        int prev = thePoints[0];
        int current = thePoints[0];
        List<int> orderedList = new List<int>();
        orderedList.Add(start);

        for (int j = 0; j < 3; ++j)
        {
            if (adjIndices[current][j] != prev &&
                point[adjIndices[current][j]].inMeta == point[current].inMeta)
            {
                prev = current;
                current = adjIndices[current][j];
                j = -1;
                if (current == start)
                {
                    break;
                }
                orderedList.Add(current);
            }
        }

        List<int> positions = AddPoints(orderedList, point, ref verts);

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

    private static bool Loop(List<int> theVertices, Points[] point)
    {
        int start = theVertices[0];
        int prev = theVertices[0];
        int current = theVertices[0];
        bool loop = false;

        for (int j = 0; j < 3; ++j)
        {
            if (adjIndices[current][j] != prev &&
                point[adjIndices[current][j]].inMeta == point[current].inMeta)
            {
                prev = current;
                current = adjIndices[current][j];
                j = -1;
                if (current == start)
                {
                    loop = true;
                    break;
                }
            }
        }

        return loop;
    }

    private static void ThreeTog(Points[] point, List<int> thePoints, ref List<Vector3> verts, ref List<int> tris, bool reverse)
    {
        int test = 1;
        int middle = 2;
        bool once = false;

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
        List<int> orderedPoints = new List<int>();
        orderedPoints.Add(thePoints[middle]);
        int second = 0;
        int third = 0;
        bool change = true;

        for (int i = 0; i < 3; ++i)
        {
            if (once && point[adjIndices[thePoints[middle]][i]].inMeta == point[thePoints[middle]].inMeta)
            {
                third = adjIndices[thePoints[middle]][i];
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


        List<int> positions = AddPoints(orderedPoints, point, ref verts);

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

    private static List<int> AddPoints(List<int> theVertices, Points[] point, ref List<Vector3> verts)
    {
        Points point1;
        Points point2;
        List<int> output = new List<int>();

        for (int i = 0; i < theVertices.Count; ++i)
        {
            point1 = point[theVertices[i]];
            for (int j = 0; j < 3; ++j)
            {
                point2 = point[adjIndices[theVertices[i]][j]];
                if (point1.inMeta != point2.inMeta)
                {
                    Vector3 dir = point2.position - point1.position;
                    dir = point1.position + dir / 2;
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

    private static List<int> ConnectHelp(int start, ref List<int> next, Points[] point, ref List<int> tested)
    {
        List<int> output = new List<int>();
        bool meta = point[start].inMeta;
        next.Remove(start);

        for (int i = 0; i < 3; ++i)
        {
            if (!tested.Contains(adjIndices[start][i]) && point[adjIndices[start][i]].inMeta == meta)
            {
                tested.Add(adjIndices[start][i]);
                output.AddRange(ConnectHelp(adjIndices[start][i], ref next, point, ref tested));
                output.Add(adjIndices[start][i]);
            }
        }
        return output;

    }

    private static Dictionary<int, List<int>> Connected(List<int> theVertices, Points[] point)
    {
        Dictionary<int, List<int>> connects = new Dictionary<int, List<int>>();
        List<int> next = new List<int>(theVertices);
        List<int> tested = new List<int>();
        List<int> connectors = new List<int>();

        int start;
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

    private static void ajacent(Points[] point, Vector2Int thePoints, ref List<Vector3> verts, ref List<int> tris, bool reverse)
    {
        int start1 = 0;
        int start2 = 0;
        List<int> temTris = new List<int>();

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

        for (int i = start1 + 1; i < start1 + 3; ++i)
        {
            Vector3 dir = point[adjIndices[thePoints[0]][i % 3]].position - point[thePoints[0]].position;
            dir = point[thePoints[0]].position + dir / 2;
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
        for (int i = start2 + 1; i < start2 + 3; ++i)
        {
            Vector3 dir = point[adjIndices[thePoints[1]][i % 3]].position - point[thePoints[1]].position;
            dir = point[thePoints[1]].position + dir / 2;
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

    private static void onePoint(Points[] point, int thePoint, ref List<Vector3> verts, ref List<int> tris, bool reverse)
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
        for (int i = start; i != end; i += increment)
        {
            Vector3 dir = point[adjIndices[thePoint][i]].position - point[thePoint].position;
            dir = point[thePoint].position + dir / 2;
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
}
