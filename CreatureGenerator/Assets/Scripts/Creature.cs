using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature
{

    private enum Type { Insect, Mammal};
    private enum Size { Small, Medium, Large}

    //main points of creature
    public Dictionary<string, List<List<Vector3>>> Points = new Dictionary<string, List<List<Vector3>>>();
    public Vector3 Start = new Vector3(0.0f, 0.0f, 0.0f);

    //Parameters
    private int Torso = 1;
    private int Head = 1;
    private int Neck = 1;
    private int ArmPairs = 1;
    private int LegPairs = 3;
    private Type LegType = Type.Mammal;
    private Size LegSize = Size.Medium;
    private int tail = 0;

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

            switch(LegType)
            {
                case Type.Insect:

                    break;
                case Type.Mammal:
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
                    else if(LegSize == Size.Medium)
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
                    break;
            }

            middle.z -= 1.5f;
        }
    }

}
