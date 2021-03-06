﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Creature
{
    //Enum's for creature types
    public enum Type { Insect, Mammal };
    public enum TypeTail { Horse, Dog, Monkey, Cat };
    public enum TypeTorso { Straight, Hunch };
    public enum Size { Small, Medium, Large, XL };
    public enum TypeHead { Monkey, Dog };

    //main points of creature
    public Dictionary<string, List<List<Vector3>>> Points = new Dictionary<string, List<List<Vector3>>>();
    public Vector3 Start = new Vector3(0.0f, 0.0f, 0.0f);

    //Parameters
    public int Head = 3;
    public TypeHead HeadType = TypeHead.Dog;
    public int neckLength = 4;
    public int TorsoSize = 8;
    public TypeTorso TorsoType = TypeTorso.Straight;
    public int ArmPairs = 2;
    public Size ArmSize = Size.Small;
    public int LegPairs = 1;
    public Type LegType = Type.Insect;
    public Size LegSize = Size.Medium;
    public int Tail = 7;
    public int TailLength = 20;
    public TypeTail TailType = TypeTail.Monkey;

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

    //Method to generate creature points
    public void Generate()
    {
        Points["Torso"].Clear();
        Points["Head"].Clear();
        Points["Neck"].Clear();
        Points["Arm"].Clear();
        Points["Leg"].Clear();
        Points["Tail"].Clear();
        Points["Spine"].Clear();
        MakeTorso();
        MakeArms();
        MakeHeads();
        MakeLegs();
        MakeTails();
    }

    public void Randomise()
    {
        System.Random rnd = new System.Random();

        //head
        Head = rnd.Next(1, 5);

        //head Type
        HeadType = (TypeHead)rnd.Next(0, 1);

        //neck Length
        neckLength = rnd.Next(0, 20);

        //Make sure all heads are visable
        if (Head != 1 && neckLength < Head * 3)
        {
            neckLength = (int)(Head * 1.5f);
        }

        //torso size
        TorsoSize = rnd.Next(0, 20);

        //torso type
        TorsoType = (TypeTorso)rnd.Next(0, 1);

        //arm pairs
        ArmPairs = rnd.Next(0, 4);

        //arm size
        ArmSize = (Size)rnd.Next(0, 3);

        //Stop arms intersecting with legs
        if (TorsoSize < 5 && ArmPairs > 0 && LegType == Type.Insect)
        {
            TorsoSize = 8;
        }

        //leg pairs
        LegPairs = rnd.Next(1, 4);

        //leg type
        LegType = (Type)rnd.Next(0, 1);

        //leg size
        LegSize = (Size)rnd.Next(0, 3);

        //tail
        Tail = rnd.Next(0, 7);

        //tail length
        TailLength = rnd.Next(1, 20);

        //tail type
        TailType = (TypeTail)rnd.Next(0, 3);
    }

    private void MakeArms()
    {
        if (ArmPairs == 0)
        {
            return;
        }

        //set arm length based on arm size
        int amount;
        if (ArmSize == Size.Small)
        {
            amount = 2;
        }
        else if (ArmSize == Size.Medium)
        {
            amount = 3;
        }
        else if (ArmSize == Size.Large)
        {
            amount = 5;
        }
        else
        {
            amount = 8;
        }
        //Loop through arm pairs
        for (int i = 0; i < ArmPairs; ++i)
        {
            //initialise lists
            Points["Arm"].Add(new List<Vector3>());
            Points["Arm"].Add(new List<Vector3>());
            //get shoulders
            int index = Points["Torso"][0].Count - (i * 2) - 1;
            Vector3 thePoint = Points["Torso"][0][index];
            Vector3 altPoint = Points["Torso"][0][index - 1];

            //start arms
            thePoint.x += 0.6f;
            altPoint.x -= 0.4f;
            Points["Arm"][Points["Arm"].Count - 2].Add(thePoint);
            Points["Arm"][Points["Arm"].Count - 1].Add(altPoint);

            //arm part one
            for (int j = 0; j < amount; ++j)
            {
                thePoint.x += 0.3f;
                altPoint.x -= 0.3f;
                thePoint.y -= 0.1f;
                altPoint.y -= 0.1f;
                Points["Arm"][Points["Arm"].Count - 2].Add(thePoint);
                Points["Arm"][Points["Arm"].Count - 1].Add(altPoint);
            }

            //arm part two
            for (int j = 0; j < amount; ++j)
            {
                thePoint.z += 0.3f;
                thePoint.y -= 0.1f;
                altPoint.z += 0.3f;
                altPoint.y -= 0.1f;
                Points["Arm"][Points["Arm"].Count - 2].Add(thePoint);
                Points["Arm"][Points["Arm"].Count - 1].Add(altPoint);
            }

            ///Hands///
            thePoint.z += 0.4f;
            thePoint.x += 0.3f;
            altPoint.z += 0.4f;
            altPoint.x += 0.3f;
            Points["Arm"][Points["Arm"].Count - 2].Add(thePoint);
            Points["Arm"][Points["Arm"].Count - 1].Add(altPoint);

            thePoint.x -= 0.6f;
            altPoint.x -= 0.6f;
            Points["Arm"][Points["Arm"].Count - 2].Add(thePoint);
            Points["Arm"][Points["Arm"].Count - 1].Add(altPoint);

            thePoint.z += 0.4f;
            thePoint.x -= 0.3f;
            altPoint.z += 0.4f;
            altPoint.x -= 0.3f;
            Points["Arm"][Points["Arm"].Count - 2].Add(thePoint);
            Points["Arm"][Points["Arm"].Count - 1].Add(altPoint);

            thePoint.x += 0.9f;
            altPoint.x += 0.9f;
            Points["Arm"][Points["Arm"].Count - 2].Add(thePoint);
            Points["Arm"][Points["Arm"].Count - 1].Add(altPoint);
            ///      ///
        }
    }

    //Method to make creature heads
    private void MakeHeads()
    {
        Vector3 thePoint;

        //Loop through heads
        for (int i = 0; i < Head; ++i)
        {

            // make start point
            if (Points["Torso"].Count > 0)
            {
                thePoint = Points["Torso"][0][Points["Torso"][0].Count - 1];
            }
            else
            {
                thePoint = Start;
            }
            if (ArmPairs > 0)
            {
                thePoint.x = (thePoint.x + Points["Torso"][0][Points["Torso"][0].Count - 2].x) / 2;
            }
            
            //Initialise necks and heads
            Points["Neck"].Add(new List<Vector3>());
            Points["Head"].Add(new List<Vector3>());

            //calculate rotation
            float angle = (Mathf.PI / (Head + 1)) * (i + 1);
            Vector2 dir = new Vector2(1, 0);
            dir = new Vector2(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle), dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle));

            //Change necks depending on torso type
            if (TorsoType == TypeTorso.Hunch)
            {
                thePoint.z += 0.5f;
                if (neckLength != 0)
                {
                    thePoint.x += 0.3f * dir.x;
                    Points["Neck"][i].Add(thePoint);
                }

            }
            for (int j = 0; j < neckLength; ++j)
            {
                thePoint.z += 1.0f * Mathf.Sin(((float)j / (float)(neckLength - 1)) * Mathf.PI / 8.0f);
                thePoint.y += 0.5f * dir.y;
                thePoint.x += 0.5f * dir.x;
                Points["Neck"][i].Add(thePoint);
            }

            //Create Head based on headtype
            switch (HeadType)
            {
                case TypeHead.Monkey:
                    thePoint.y += 0.5f;
                    thePoint.z += 0.2f;
                    Points["Head"][i].Add(thePoint);
                    thePoint.z += 0.3f;
                    thePoint.x += 0.2f;
                    Points["Head"][i].Add(thePoint);
                    thePoint.x -= 0.4f;
                    Points["Head"][i].Add(thePoint);
                    thePoint.y += 0.5f;
                    thePoint.z -= 0.1f;
                    thePoint.x += 0.2f;
                    Points["Head"][i].Add(thePoint);
                    break;
                case TypeHead.Dog:
                    thePoint.y += 0.5f;
                    thePoint.z += 0.2f;
                    Points["Head"][i].Add(thePoint);
                    thePoint.y += 0.5f;
                    Points["Head"][i].Add(thePoint);
                    thePoint.y -= 0.5f;
                    thePoint.z += 0.5f;
                    Points["Head"][i].Add(thePoint);
                    thePoint.z += 0.5f;
                    Points["Head"][i].Add(thePoint);
                    break;
            }
        }
    }

    //Method to to make creature torso
    private void MakeTorso()
    {

        if (TorsoSize == 0 && ArmPairs == 0) { return; }

        //initialise torso list
        Points["Torso"].Add(new List<Vector3>());

        Vector3 thePoint = Start;
        int amount = ArmPairs;

        //make sure there is a Torso has shoulders even if there are no arms
        if (amount == 0)
        {
            amount = 1;
        }

        //Do work based on torso type
        switch (TorsoType)
        {
            case TypeTorso.Straight:
            {
                //make points of torso
                for (int i = 0; i < TorsoSize - amount; ++i)
                {
                    thePoint.y += 0.5f;
                    //make torso curve slightly
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

                //create shoulders
                for (int j = 0; j < amount; ++j)
                {
                    thePoint.y += 1.2f;
                    thePoint.x -= 0.5f;
                    Points["Torso"][0].Add(thePoint);
                    thePoint.x *= -1.0f;
                    Points["Torso"][0].Add(thePoint);
                }

                break;
            }
            case TypeTorso.Hunch:
            {

                //create hunched torso points
                for (int i = 0; i < TorsoSize - amount; ++i)
                {
                    thePoint.y += 0.5f;
                    thePoint.z += 0.3f * Mathf.Sin(((float)i / (float)(TailLength - 1)) * Mathf.PI / 8.0f);
                    Points["Torso"][0].Add(thePoint);
                }

                //create hunched shoulders
                for (int j = 0; j < amount; ++j)
                {
                    thePoint.y += 1.2f;
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

    //Method to make creature legs
    private void MakeLegs()
    {
        //initialise legs
        Vector3 middle = Start;
        Points["Spine"].Add(new List<Vector3>());
        float metaDistance = 0.8f;

        //set leg length
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

        //for all legs
        for (int i = 0; i < LegPairs; ++i)
        {
            //Lengthen spine
            if (middle != Start)
            {
                Points["Spine"][0].Add(new Vector3(middle.x, middle.y, middle.z + 0.75f));
                Points["Spine"][0].Add(middle);
            }

            Vector3 thePoint = middle;
            Vector3 mirrorPoint = middle;
            thePoint.x += 0.5f;
            mirrorPoint.x -= 0.5f;

            //add hips
            Points["Leg"].Add(new List<Vector3>());
            Points["Leg"].Add(new List<Vector3>());
            Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
            Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);

            //check leg type
            switch (LegType)
            {
                case Type.Insect:
                {
                    //move point
                    thePoint.y += metaDistance;
                    thePoint.x += 0.5f;

                    //start both legs
                    mirrorPoint = thePoint;
                    mirrorPoint.x = -thePoint.x;
                    Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                    Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);

                    //make first section of insect legs
                    for (int j = 0; j < amount; ++j)
                    {
                        thePoint.y += metaDistance / 2;
                        thePoint.x += Mathf.Cos((float)j / ((float)amount * 1.5f) * Mathf.PI / 1.5f) - 0.5f;
                        thePoint.z += 0.2f;

                        mirrorPoint = thePoint;
                        mirrorPoint.x = -thePoint.x;
                        Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                        Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);
                    }

                    //make second section of insect leg
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
                    //move point
                    thePoint.y -= metaDistance;
                    thePoint.x += 0.3f;

                    //start both legs
                    mirrorPoint = thePoint;
                    mirrorPoint.x = -thePoint.x;
                    Points["Leg"][Points["Leg"].Count - 1].Add(thePoint);
                    Points["Leg"][Points["Leg"].Count - 2].Add(mirrorPoint);

                    //make first section of mammal legs
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

                    //make second section of mammal legs
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

                    //add feet
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

    //Method to make creature tails
    private void MakeTails()
    {
        //don't make tail if length is 0
        if (Tail == 0 || TailLength == 0)
        {
            return;
        }

        Vector3 thePoint;

        for (int i = 0; i < Tail; ++i)
        {
            //calculate angle for tail
            float angle = (Mathf.PI / (Tail + 1)) * (i + 1);
            Vector2 dir = new Vector2(1, 0);
            dir = new Vector2(dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle), dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle));

            //initialise tail
            Points["Tail"].Add(new List<Vector3>());

            //set tail start
            if (Points["Spine"][0].Count != 0)
            {
                thePoint = Points["Spine"][0][Points["Spine"][0].Count - 1];
            }
            else
            {
                thePoint = new Vector3(Start.x, Start.y + 0.2f, Start.z - 0.1f);
            }

            //check tail type
            switch (TailType)
            {
                case TypeTail.Horse:
                {
                    thePoint.z -= 0.5f;
                    thePoint.y += 0.3f;
                    //make tail the right length
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

                    //make tail the right length
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
                    TailLoop(thePoint, dir, i, -1);
                    break;
                }
                case TypeTail.Cat:
                {
                    TailLoop(thePoint, dir, i, 1);
                    break;
                }
            }
        }
    }

    ///Helper method for creature tails with loops
    private void TailLoop(Vector3 thePoint, Vector2 dir, int i, int direction)
    {
        thePoint.z -= 0.1f;
        thePoint.y += 0.3f;

        //make the tail the right length
        for (int j = 0; j < TailLength - 2; ++j)
        {
            thePoint.y += (0.6f * Mathf.Sin(((float)j / (float)(TailLength - 1)) * Mathf.PI / 4.0f)) * dir.y;
            thePoint.z -= 0.3f * Mathf.Cos(((float)j / (float)(TailLength - 1)) * Mathf.PI / 4.0f);
            thePoint.x += 0.3f * dir.x;

            Points["Tail"][i].Add(thePoint);
        }

        //make the last ones straight to go into loop
        for (int j = 0; j < (TailLength - (TailLength - 2)); ++j)
        {
            thePoint.z -= (1.0f - (float)j) / 5.0f;
            thePoint.y += 0.4f * dir.y;
            thePoint.x += 0.3f * dir.x;
            Points["Tail"][i].Add(thePoint);
        }


        ///// TAIL LOOP ///
        int end = Points["Tail"][i].Count - 1;
        int loop = 10;
        Vector3 first = new Vector3(0.0f, 0.0f, 0.0f);

        for (int j = 0; j < loop; ++j)
        {
            Vector3 origin = Points["Tail"][i][end];

            //calculate angle for loop
            float ang = 2.0f * Mathf.PI + (Mathf.PI * (float)j / (float)(loop - 1));
            float r;

            //create radius based on taillength to a max of 13
            if (TailLength < 9)
            {
                r = ((((1.0f - (float)j / (float)(loop - 1)))) * ang) * (float)TailLength / 50.0f;
            }
            else
            {
                r = ((((1.0f - (float)j / (float)(loop - 1)))) * ang) * 9.0f / 40.0f;
            }
            //set values of the first point
            if (j == 0)
            {
                first.z = r * direction * Mathf.Cos(ang);
                first.y = r * Mathf.Sin(ang);
            }

            //move point to right position
            origin -= first;
            origin.z += r * direction * Mathf.Cos(ang);
            origin.y += (r * Mathf.Sin(ang)) * dir.y;
            origin.x += 0.3f * dir.x;

            //add point if not the first one
            if (j != 0)
            {
                Points["Tail"][i].Add(origin);
            }
        }
    }
}
