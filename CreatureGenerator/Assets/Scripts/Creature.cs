using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature
{

    private enum Type { Insect, Mammal };
    private enum TypeTail { Horse, Dog, Monkey, Cat }
    private enum Size { Small, Medium, Large }

    //main points of creature
    public Dictionary<string, List<List<Vector3>>> Points = new Dictionary<string, List<List<Vector3>>>();
    public Vector3 Start = new Vector3(0.0f, 0.0f, 0.0f);

    //Parameters
    private int Head = 1;
    private int ArmPairs = 1;
    private int LegPairs = 3;
    private Type LegType = Type.Insect;
    private Size LegSize = Size.Small;
    private int Tail = 5;
    private int TailLength = 4;
    private TypeTail TailType = TypeTail.Dog;

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
        MakeLegs();
        MakeTails();
    }

    private void MakeLegs()
    {
        Vector3 middle = Start;
        Points["Spine"].Add(new List<Vector3>());

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

            switch (LegType)
            {
                case Type.Insect:
                    {
                        thePoint.y += 0.3f;
                        thePoint.x += 0.3f;

                        mirrorPoint = thePoint;
                        mirrorPoint.x = -thePoint.x;
                        Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                        Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);

                        int amount;
                        if (LegSize == Size.Small)
                        {
                            amount = 3;
                        }
                        else if (LegSize == Size.Medium)
                        {
                            amount = 5;
                        }
                        else
                        {
                            amount = 8;
                        }


                        for (int j = 0; j < amount / 2; ++j)
                        {
                            thePoint.y += 0.3f;
                            thePoint.x += 0.3f;
                            thePoint.z += 0.1f;

                            mirrorPoint = thePoint;
                            mirrorPoint.x = -thePoint.x;
                            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                        }

                        for (int j = 0; j < amount * 1.5; ++j)
                        {
                            thePoint.y -= 0.3f;
                            thePoint.x += 0.0f + Mathf.Sin((float)j / ((float)amount * 1.5f) * Mathf.PI / 8);
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
                        thePoint.y -= 0.3f;
                        thePoint.x += 0.3f;

                        mirrorPoint = thePoint;
                        mirrorPoint.x = -thePoint.x;
                        Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                        Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);

                        int amount;
                        if (LegSize == Size.Small)
                        {
                            amount = 2;
                        }
                        else if (LegSize == Size.Medium)
                        {
                            amount = 5;
                        }
                        else
                        {
                            amount = 8;
                        }


                        for (int j = 0; j < amount; ++j)
                        {
                            thePoint.y -= 0.3f;
                            thePoint.x += 0.1f;

                            mirrorPoint = thePoint;
                            mirrorPoint.x = -thePoint.x;
                            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                        }

                        thePoint.z += 0.5f;

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
        for (int i = 0; i < Tail; ++i)
        {
            switch (TailType)
            {
                case TypeTail.Horse:
                    {
                        Points["Tail"].Add(new List<Vector3>());
                        Vector3 thePoint;
                        if (Points["Spine"].Count != 0)
                        {
                            thePoint = Points["Spine"][0][Points["Spine"][0].Count - 1];
                        }
                        else
                        {
                            thePoint = new Vector3(Start.x, Start.y + 0.2f, Start.z - 0.1f);
                        }
                        thePoint.z -= 0.3f;
                        thePoint.y += 0.3f;
                        for (int j = 0; j < TailLength; ++j)
                        {
                            thePoint.y -= 0.3f;
                            thePoint.z -= 0.2f;

                            if (i > (int)(Tail / 2))
                            {
                                thePoint.x += ((Tail - ((float)i % Tail)) / 2.0f) * 0.5f;
                            }
                            else
                            {
                                thePoint.x -= ((float)i / 2.0f) * 0.5f;
                            }

                            Points["Tail"][i].Add(thePoint);

                        }
                        break;
                    }
                case TypeTail.Dog:
                    {
                        Points["Tail"].Add(new List<Vector3>());
                        Vector3 thePoint;
                        if (Points["Spine"].Count != 0)
                        {
                            thePoint = Points["Spine"][0][Points["Spine"][0].Count - 1];
                        }
                        else
                        {
                            thePoint = new Vector3(Start.x, Start.y + 0.2f, Start.z - 0.1f);
                        }
                        thePoint.z -= 0.3f;
                        thePoint.y += 0.3f;
                        for (int j = 0; j < TailLength; ++j)
                        {
                            thePoint.y += 0.3f;
                            thePoint.z -= 0.2f;

                            if (i > (int)(Tail / 2))
                            {
                                thePoint.x += ((Tail - ((float)i % Tail)) / 2.0f) * 0.5f;
                            }
                            else
                            {
                                thePoint.x -= ((float)i / 2.0f) * 0.5f;
                            }

                            Points["Tail"][i].Add(thePoint);

                        }
                        break;
                    }
            }
        }
    }
}
