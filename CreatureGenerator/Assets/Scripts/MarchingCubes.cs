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
        new int[]{0,3,6 }, //2
        new int[]{7,2,1 }, //3
        new int[]{6,5,0 }, //4
        new int[]{1,4,7 }, //5
        new int[]{7,4,2 }, //6
        new int[]{3,5,6 }  //7
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
                    if (Connected(theVertices, point)[0].x == 3)
                    {

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
                    break;
                case 5:
                    if (adjIndices[notIn[0]][0] == notIn[1] ||
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

    private static int ConnectHelp(int start, ref List<int> next, Points[] point, ref List<int> tested)
    {
        bool meta = point[start].inMeta;
        int count = 0;
        next.Remove(start);

        for (int i = 0; i < 3; ++i)
        {
            if (!tested.Contains(adjIndices[start][i]) && point[adjIndices[start][i]].inMeta == meta)
            {
                tested.Add(adjIndices[start][i]);
                count += 1 + ConnectHelp(adjIndices[start][i], ref next, point, ref tested);
            }
        }
        return count;

    }

    private static List<Vector2> Connected(List<int> theVertices, Points[] point)
    {
        List<Vector2> connects = new List<Vector2>();
        List<int> next = new List<int>(theVertices);
        List<int> tested = new List<int>();

        int start;
        while (next.Count > 0)
        {
            start = next[0];
            connects.Add(new Vector2(ConnectHelp(start, ref next, point, ref tested), start));
            tested.Clear();
        }

        return connects;
    }

    private static void ajacent(Points[] point, Vector2Int thePoints, ref List<Vector3> verts, ref List<int> tris, bool reverse)
    {
        int start1 = 0;
        int start2 = 0;
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
            dir /= 2;
            verts.Add(point[thePoints[0]].position + dir);
        }
        for (int i = start2 + 1; i < start2 + 3; ++i)
        {
            Vector3 dir = point[adjIndices[thePoints[1]][i % 3]].position - point[thePoints[1]].position;
            dir /= 2;
            verts.Add(point[thePoints[1]].position + dir);
        }

        if (reverse)
        {
            tris.Add(verts.Count - 1);
            tris.Add(verts.Count - 3);
            tris.Add(verts.Count - 4);
            tris.Add(verts.Count - 1);
            tris.Add(verts.Count - 2);
            tris.Add(verts.Count - 3);
        }
        else
        {
            tris.Add(verts.Count - 4);
            tris.Add(verts.Count - 3);
            tris.Add(verts.Count - 1);
            tris.Add(verts.Count - 3);
            tris.Add(verts.Count - 2);
            tris.Add(verts.Count - 1);
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
            dir /= 2;
            verts.Add(point[thePoint].position + dir);
            tris.Add(verts.Count - 1);
        }
    }
}
