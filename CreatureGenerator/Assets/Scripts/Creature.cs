using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature
{

    private enum Type { Insect, Mammal };
    private enum TypeTail { Horse, Dog, Monkey, Cat };
    private enum TypeTorso { Straight, Hunch };
    private enum Size { Small, Medium, Large, XL };

    //main points of creature
    public Dictionary<string, List<List<Vector3>>> Points = new Dictionary<string, List<List<Vector3>>>();
    public Vector3 Start = new Vector3(0.0f, 0.0f, 0.0f);

    //Parameters
    private int Head = 1;
    private int neckLength = 3;
    private int TorsoSize = 6;
    private TypeTorso TorsoType = TypeTorso.Hunch;
    private int ArmPairs = 2;
    private int LegPairs = 3;
    private Type LegType = Type.Mammal;
    private Size LegSize = Size.Medium;
    private int Tail = 5;
    private int TailLength = 8;
    private TypeTail TailType = TypeTail.Cat;

    //constructor
    public Creature()
    {
        //initialise body parts
        Points.Add("Torso", new List<List<Vector3>>());
        Points.Add("Head", new List<List<Vector3>>());
        Points.Add("Neck", new List<List<Vector3>>());
        Points.Add("Arm", new List<List<Vector3>>());
        Points.Add("Leg", new List<List<Vector3>>());
        Points.Add("Tail", new List<List<Vector3>>());
        Points.Add("Spine", new List<List<Vector3>>());
    }

    public void Generate()
    {
        MakeTorso();
        MakeHeads();
        MakeLegs();
        MakeTails();
    }

    private void MakeHeads()
    {
        Points["Head"].Add(new List<Vector3>());
        Vector3 thePoint;

        if (Points["Torso"][0].Count == 0)
        {
            thePoint = Start;
        }
        else
        {
            thePoint = Points["Torso"][0][Points["Torso"][0].Count - 1];
        }

        for (int i = 0; i < Head; ++i)
        {
            Points["Neck"].Add(new List<Vector3>());
            float angle = (Mathf.PI / (Tail + 1)) * (i + 1);
            Vector2 dir = new Vector2(1, 0);
            dir = new Vector2(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle), dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle));

            if (TorsoType == TypeTorso.Hunch)
            {
                thePoint.z += 0.5f;
                if (neckLength != 0)
                {
                    Points["Neck"][i].Add(thePoint);
                }

            }
            for (int j = 0; j < neckLength; ++j)
            {
                thePoint.z += 1.0f * Mathf.Sin(((float)j / (float)(TailLength - 1)) * Mathf.PI / 8.0f);
                thePoint.y += 0.5f;
                Points["Neck"][i].Add(thePoint);
            }




        }
    }

    private void MakeTorso()
    {
        Points["Torso"].Add(new List<Vector3>());
        switch (TorsoType)
        {
            case TypeTorso.Straight:
                {
                    Vector3 thePoint = Start;
                    int amount = ArmPairs;
                    if (amount == 0)
                    {
                        amount = 1;
                    }

                    for (int i = 0; i < TorsoSize - amount; ++i)
                    {
                        thePoint.y += 0.5f;
                        if (i < (TorsoSize - amount) / 2.0f)
                        {
                            thePoint.z += (float)i / 100.0f;
                        }
                        else
                        {
                            thePoint.z -= (float)i / 100.0f;
                        }
                        Points["Torso"][0].Add(thePoint);
                    }


                    for (int j = 0; j < amount; ++j)
                    {
                        thePoint.y += 0.5f;
                        thePoint.x -= 0.5f;
                        Points["Torso"][0].Add(thePoint);
                        thePoint.x *= -1.0f;
                        Points["Torso"][0].Add(thePoint);
                    }

                    break;
                }
            case TypeTorso.Hunch:
                {
                    Vector3 thePoint = Start;
                    int amount = ArmPairs;
                    if (amount == 0)
                    {
                        amount = 1;
                    }

                    for (int i = 0; i < TorsoSize - amount; ++i)
                    {
                        thePoint.y += 0.4f;
                        thePoint.z += 0.3f * Mathf.Sin(((float)i / (float)(TailLength - 1)) * Mathf.PI / 8.0f);
                        Points["Torso"][0].Add(thePoint);
                    }


                    for (int j = 0; j < amount; ++j)
                    {
                        thePoint.y += 0.5f;
                        thePoint.x -= 0.5f;
                        thePoint.z += 0.8f * Mathf.Sin((((float)TorsoSize + (float)j) / (float)(TorsoSize + amount)) * Mathf.PI / 8.0f);
                        Points["Torso"][0].Add(thePoint);
                        thePoint.x *= -1.0f;
                        Points["Torso"][0].Add(thePoint);
                    }

                    break;
                }
        }
    }

    private void MakeLegs()
    {
        Vector3 middle = Start;
        Points["Spine"].Add(new List<Vector3>());
        float metaDistance = 0.8f;

        for (int i = 0; i < LegPairs; ++i)
        {
            if (middle != Start)
            {
                Points["Spine"][0].Add(new Vector3(middle.x, middle.y, middle.z + 0.75f));
                Points["Spine"][0].Add(middle);
            }

            Vector3 thePoint = middle;
            Vector3 mirrorPoint = middle;
            thePoint.x += 0.5f;
            mirrorPoint.x -= 0.5f;

            Points["Leg"].Add(new List<Vector3>());
            Points["Leg"].Add(new List<Vector3>());
            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);

            int amount;
            if (LegSize == Size.Small)
            {
                amount = 2;
            }
            else if (LegSize == Size.Medium)
            {
                amount = 3;
            }
            else if (LegSize == Size.Large)
            {
                amount = 5;
            }
            else
            {
                amount = 8;
            }

            switch (LegType)
            {
                case Type.Insect:
                    {
                        thePoint.y += metaDistance;
                        thePoint.x += 0.5f;

                        mirrorPoint = thePoint;
                        mirrorPoint.x = -thePoint.x;
                        Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                        Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);


                        for (int j = 0; j < amount; ++j)
                        {
                            thePoint.y += metaDistance / 2;
                            thePoint.x += Mathf.Cos((float)j / ((float)amount * 1.5f) * Mathf.PI / 1.5f) - 0.5f;
                            thePoint.z += 0.1f;

                            mirrorPoint = thePoint;
                            mirrorPoint.x = -thePoint.x;
                            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                        }

                        for (int j = 0; j < amount * 1.5; ++j)
                        {
                            thePoint.y -= metaDistance;
                            thePoint.x += 0.1f + Mathf.Sin((float)j / ((float)amount * 1.5f) * Mathf.PI / 8);
                            thePoint.z += 0.0f + Mathf.Sin((float)j / ((float)amount * 1.5f) * Mathf.PI / 16);

                            mirrorPoint = thePoint;
                            mirrorPoint.x = -thePoint.x;
                            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                        }
                        break;
                    }
                case Type.Mammal:
                    {
                        thePoint.y -= metaDistance;
                        thePoint.x += 0.3f;

                        mirrorPoint = thePoint;
                        mirrorPoint.x = -thePoint.x;
                        Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                        Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);


                        for (int j = 0; j < amount / 2; ++j)
                        {
                            thePoint.y -= metaDistance;
                            thePoint.x += 0.1f;
                            thePoint.z += 0.1f;

                            mirrorPoint = thePoint;
                            mirrorPoint.x = -thePoint.x;
                            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                        }

                        for (int j = 0; j < amount / 2; ++j)
                        {
                            thePoint.y -= metaDistance;
                            thePoint.x += 0.1f;
                            thePoint.z -= 0.1f;

                            mirrorPoint = thePoint;
                            mirrorPoint.x = -thePoint.x;
                            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                        }

                        //feet
                        thePoint.z += 0.4f;

                        mirrorPoint = thePoint;
                        mirrorPoint.x = -thePoint.x;
                        Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                        Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                    }
                    break;
            }

            middle.z -= 1.5f;
        }
    }

    private void MakeTails()
    {
        if (Tail == 0 || TailLength == 0)
        {
            return;
        }
        for (int i = 0; i < Tail; ++i)
        {
            float angle = (Mathf.PI / (Tail + 1)) * (i + 1);
            Vector2 dir = new Vector2(1, 0);
            dir = new Vector2(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle), dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle));

            Points["Tail"].Add(new List<Vector3>());
            Vector3 thePoint;

            if (Points["Spine"][0].Count != 0)
            {
                thePoint = Points["Spine"][0][Points["Spine"][0].Count - 1];
            }
            else
            {
                thePoint = new Vector3(Start.x, Start.y + 0.2f, Start.z - 0.1f);
            }

            switch (TailType)
            {
                case TypeTail.Horse:
                    {
                        thePoint.z -= 0.5f;
                        thePoint.y += 0.3f;
                        for (int j = 0; j < TailLength; ++j)
                        {
                            thePoint.y -= 0.3f * dir.y;
                            thePoint.z -= 0.1f;
                            thePoint.x += 0.3f * dir.x;

                            Points["Tail"][i].Add(thePoint);

                        }
                        break;
                    }
                case TypeTail.Dog:
                    {
                        thePoint.z -= 0.3f;
                        thePoint.y += 0.3f;
                        for (int j = 0; j < TailLength; ++j)
                        {
                            thePoint.y += 0.3f * dir.y;
                            thePoint.z -= 0.2f;
                            thePoint.x += 0.3f * dir.x;


                            Points["Tail"][i].Add(thePoint);

                        }
                        break;
                    }
                case TypeTail.Monkey:
                    {
                        thePoint.z -= 0.1f;
                        thePoint.y += 0.3f;
                        for (int j = 0; j < TailLength - 2; ++j)
                        {
                            thePoint.y += (0.6f * Mathf.Sin(((float)j / (float)(TailLength - 1)) * Mathf.PI / 4.0f)) * dir.y;
                            thePoint.z -= 0.3f * Mathf.Cos(((float)j / (float)(TailLength - 1)) * Mathf.PI / 4.0f);
                            thePoint.x += 0.3f * dir.x;

                            Points["Tail"][i].Add(thePoint);
                        }

                        for (int j = 0; j < (TailLength - (TailLength - 2)); ++j)
                        {
                            thePoint.z -= (1.0f - (float)j) / 5.0f;
                            thePoint.y += 0.4f * dir.y;
                            thePoint.x += 0.3f * dir.x;
                            Points["Tail"][i].Add(thePoint);
                        }


                        //LOOP
                        int end = Points["Tail"][i].Count - 1;
                        int loop = 10;
                        Vector3 first = new Vector3(0.0f, 0.0f, 0.0f);

                        for (int j = 0; j < loop; ++j)
                        {
                            Vector3 origin = Points["Tail"][i][end];

                            float ang = 2.0f * Mathf.PI + (Mathf.PI * (float)j / (float)(loop - 1));
                            float r;
                            if (TailLength < 13)
                            {
                                r = ((((1.0f - (float)j / (float)(loop - 1)))) * ang) * (float)TailLength / 50.0f;
                            }
                            else
                            {
                                r = ((((1.0f - (float)j / (float)(loop - 1)))) * ang) * 13.0f / 40.0f;
                            }
                            if (j == 0)
                            {
                                first.z = r * -Mathf.Cos(ang);
                                first.y = r * Mathf.Sin(ang);
                            }
                            origin -= first;
                            origin.z += r * -Mathf.Cos(ang);
                            origin.y += (r * Mathf.Sin(ang)) * dir.y;
                            origin.x += 0.3f * dir.x;
                            if (j != 0)
                            {
                                Points["Tail"][i].Add(origin);
                            }
                        }
                        break;
                    }
                case TypeTail.Cat:
                    {
                        thePoint.z -= 0.1f;
                        thePoint.y += 0.3f;
                        for (int j = 0; j < TailLength - 2; ++j)
                        {
                            thePoint.y += (0.6f * Mathf.Sin(((float)j / (float)(TailLength - 1)) * Mathf.PI / 4.0f)) * dir.y;
                            thePoint.z -= 0.3f * Mathf.Cos(((float)j / (float)(TailLength - 1)) * Mathf.PI / 4.0f);
                            thePoint.x += 0.3f * dir.x;

                            Points["Tail"][i].Add(thePoint);
                        }

                        for (int j = 0; j < (TailLength - (TailLength - 2)); ++j)
                        {
                            thePoint.z -= (1.0f - (float)j) / 5.0f;
                            thePoint.y += 0.4f * dir.y;
                            thePoint.x += 0.3f * dir.x;
                            Points["Tail"][i].Add(thePoint);
                        }


                        //LOOP
                        int end = Points["Tail"][i].Count - 1;
                        int loop = 10;
                        Vector3 first = new Vector3(0.0f, 0.0f, 0.0f);

                        for (int j = 0; j < loop; ++j)
                        {
                            Vector3 origin = Points["Tail"][i][end];

                            float ang = 2.0f * Mathf.PI + (Mathf.PI * (float)j / (float)(loop - 1));
                            float r;
                            if (TailLength < 13)
                            {
                                r = ((((1.0f - (float)j / (float)(loop - 1)))) * ang) * (float)TailLength / 50.0f;
                            }
                            else
                            {
                                r = ((((1.0f - (float)j / (float)(loop - 1)))) * ang) * 13.0f / 40.0f;
                            }
                            if (j == 0)
                            {
                                first.z = r * Mathf.Cos(ang);
                                first.y = r * Mathf.Sin(ang);
                            }
                            origin -= first;
                            origin.z += r * Mathf.Cos(ang);
                            origin.y += (r * Mathf.Sin(ang)) * dir.y;
                            origin.x += 0.3f * dir.x;
                            if (j != 0)
                            {
                                Points["Tail"][i].Add(origin);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
