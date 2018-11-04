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
            if (count == 0 || count == 8)
            {
                continue;
            }

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
                case 6:
                    if (adjIndices[theVertices[0]][0] == theVertices[1] ||
                        adjIndices[theVertices[0]][1] == theVertices[1] ||
                        adjIndices[theVertices[0]][2] == theVertices[1])
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

            }
        }
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
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
        int count = 0;
        Vector3 triangle = new Vector3();
        if (thePoint % 2 == 0)
        {
            Vector3 dir = point[thePoint + 1].position - point[thePoint].position;
            float length = dir.magnitude / 2;
            dir = dir.normalized;
            verts.Add(point[thePoint].position + dir * length);
            ++count;
            triangle.x = verts.Count - 1;
        }
        else
        {
            Vector3 dir = point[thePoint - 1].position - point[thePoint].position;
            float length = dir.magnitude / 2;
            dir = dir.normalized;
            verts.Add(point[thePoint].position + dir * length);
            triangle.x = verts.Count - 1;
        }
        if (thePoint % 4 == 0 || thePoint % 4 == 1)
        {
            Vector3 dir = point[thePoint + 2].position - point[thePoint].position;
            float length = dir.magnitude / 2;
            dir = dir.normalized;
            verts.Add(point[thePoint].position + dir * length);
            ++count;
            triangle.y = verts.Count - 1;
        }
        else
        {
            Vector3 dir = point[thePoint - 2].position - point[thePoint].position;
            float length = dir.magnitude / 2;
            dir = dir.normalized;
            verts.Add(point[thePoint].position + dir * length);
            triangle.y = verts.Count - 1;
        }
        if (thePoint < 4)
        {
            Vector3 dir = point[thePoint + 4].position - point[thePoint].position;
            float length = dir.magnitude / 2;
            dir = dir.normalized;
            verts.Add(point[thePoint].position + dir * length);
            ++count;
            triangle.z = verts.Count - 1;
        }
        else
        {
            Vector3 dir = point[thePoint - 4].position - point[thePoint].position;
            float length = dir.magnitude / 2;
            dir = dir.normalized;
            verts.Add(point[thePoint].position + dir * length);
            triangle.z = verts.Count - 1;
        }
        if ((!reverse && count % 2 == 1) || (reverse && count % 2 == 0))
        {
            tris.Add((int)triangle.x);
            tris.Add((int)triangle.y);
            tris.Add((int)triangle.z);
        }
        else
        {
            tris.Add((int)triangle.z);
            tris.Add((int)triangle.y);
            tris.Add((int)triangle.x);
        }
    }
}
