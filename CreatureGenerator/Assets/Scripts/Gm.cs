using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gm : MonoBehaviour
{
    //Creatures
    private Creature c1;
    private Creature c2;
    private Creature c3;

    private float turnSpeed = 20;

    //Add points for Creature1
    private void Creature1()
    {
        c1 = new Creature();
        //start point
        c1.Start = new Vector3(0.0f, 0.0f, 0.0f);

        //torso
        c1.Points["Torso"].Add(new List<Vector3>());
        c1.Points["Torso"][0].Add(new Vector3(0.0f, 1.0f, 0.0f));   //mid torso
        c1.Points["Torso"][0].Add(new Vector3(0.0f, 2.0f, 0.0f));   //top torso

        //neck
        c1.Points["Neck"].Add(new List<Vector3>());
        c1.Points["Neck"][0].Add(new Vector3(0.0f, 2.5f, 0.0f));   //neck

        //head
        c1.Points["Head"].Add(new List<Vector3>());
        c1.Points["Head"][0].Add(new Vector3(0.0f, 3.0f, 0.2f));   //head

        //arm
        c1.Points["Arm"].Add(new List<Vector3>());
        c1.Points["Arm"].Add(new List<Vector3>());
        c1.Points["Arm"][0].Add(new Vector3(0.75f, 2.0f, 0.0f));   //shoulder 1
        c1.Points["Arm"][1].Add(new Vector3(-0.75f, 2.0f, 0.0f));  //shoulder 2
        c1.Points["Arm"][0].Add(new Vector3(1.25f, 1.0f, 0.25f));   //elbo 1
        c1.Points["Arm"][1].Add(new Vector3(-1.25f, 1.0f, 0.25f));  //elbo 2
        c1.Points["Arm"][0].Add(new Vector3(1.5f, 0.0f, 0.5f));   //hand 1
        c1.Points["Arm"][1].Add(new Vector3(-1.5f, 0.0f, 0.5f));  //hand 2

        //Leg1
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Leg"][0].Add(new Vector3(-0.5f, 0.0f, 0.0f));  //hip  front 1
        c1.Points["Leg"][1].Add(new Vector3(0.5f, 0.0f, 0.0f));   //hip  front 2
        c1.Points["Leg"][0].Add(new Vector3(-1.0f, -1.0f, 0.0f));  //knee front 1
        c1.Points["Leg"][1].Add(new Vector3(1.0f, -1.0f, 0.0f));  //knee front 2
        c1.Points["Leg"][0].Add(new Vector3(-1.1f, -2.0f, 0.0f));  //foot front 1
        c1.Points["Leg"][1].Add(new Vector3(1.25f, -2.0f, 0.0f));  //foot front 2

        //Legs 2
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Spine"].Add(new List<Vector3>());
        c1.Points["Spine"][0].Add(new Vector3(0.0f, 0.0f, -1.0f));   //spine
        c1.Points["Leg"][2].Add(new Vector3(-0.5f, 0.0f, -1.0f));  //hip  front 1
        c1.Points["Leg"][3].Add(new Vector3(0.5f, 0.0f, -1.0f));   //hip  front 2
        c1.Points["Leg"][2].Add(new Vector3(-1.0f, -1.0f, -1.0f));  //knee front 1
        c1.Points["Leg"][3].Add(new Vector3(1.0f, -1.0f, -1.0f));  //knee front 2
        c1.Points["Leg"][2].Add(new Vector3(-1.25f, -2.0f, -1.0f));  //foot front 1
        c1.Points["Leg"][3].Add(new Vector3(1.1f, -2.0f, -1.0f));  //foot front 2

        //Legs 3
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Spine"].Add(new List<Vector3>());
        c1.Points["Spine"][1].Add(new Vector3(0.0f, 0.0f, -2.0f));   //spine
        c1.Points["Leg"][4].Add(new Vector3(-0.5f, 0.0f, -2.0f));  //hip  front 1
        c1.Points["Leg"][5].Add(new Vector3(0.5f, 0.0f, -2.0f));   //hip  front 2
        c1.Points["Leg"][4].Add(new Vector3(-1.0f, -1.0f, -2.0f));  //knee front 1
        c1.Points["Leg"][5].Add(new Vector3(1.0f, -1.0f, -2.0f));  //knee front 2
        c1.Points["Leg"][4].Add(new Vector3(-1.1f, -2.0f, -2.0f));  //foot front 1
        c1.Points["Leg"][5].Add(new Vector3(1.25f, -2.0f, -2.0f));  //foot front 2

        //Legs 4
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Leg"].Add(new List<Vector3>());
        c1.Points["Spine"].Add(new List<Vector3>());
        c1.Points["Spine"][2].Add(new Vector3(0.0f, 0.0f, -3.0f));   //spine
        c1.Points["Leg"][6].Add(new Vector3(-0.5f, 0.0f, -3.0f));  //hip  front 1
        c1.Points["Leg"][7].Add(new Vector3(0.5f, 0.0f, -3.0f));   //hip  front 2
        c1.Points["Leg"][6].Add(new Vector3(-1.0f, -1.0f, -3.0f));  //knee front 1
        c1.Points["Leg"][7].Add(new Vector3(1.0f, -1.0f, -3.0f));  //knee front 2
        c1.Points["Leg"][6].Add(new Vector3(-1.25f, -2.0f, -3.0f));  //foot front 1
        c1.Points["Leg"][7].Add(new Vector3(1.1f, -2.0f, -3.0f));  //foot front 2

        //tail
        c1.Points["Tail"].Add(new List<Vector3>());
        c1.Points["Tail"][0].Add(new Vector3(0.0f, 0.5f, -3.25f));
        c1.Points["Tail"][0].Add(new Vector3(0.0f, 1.0f, -4.0f));
    }

    //Add points for creature 2
    private void Creature2()
    {
        c2 = new Creature();
        //start point
        c2.Start = new Vector3(0.0f, 0.0f, 0.0f);

        //torso
        c2.Points["Torso"].Add(new List<Vector3>());
        c2.Points["Torso"][0].Add(new Vector3(0.0f, 1.0f, 0.0f));   //mid torso

        //neck
        c2.Points["Neck"].Add(new List<Vector3>());
        c2.Points["Neck"][0].Add(new Vector3(0.0f, 1.25f, 0.0f));   //neck
        c2.Points["Neck"][0].Add(new Vector3(0.0f, 1.5f, -0.1f));   //neck
        c2.Points["Neck"][0].Add(new Vector3(0.0f, 1.75f, -0.2f));   //neck
        c2.Points["Neck"][0].Add(new Vector3(0.0f, 2.0f, -0.3f));   //neck
        c2.Points["Neck"][0].Add(new Vector3(0.0f, 2.25f, -0.2f));   //neck
        c2.Points["Neck"][0].Add(new Vector3(0.0f, 2.5f, -0.1f));   //neck

        //head
        c2.Points["Head"].Add(new List<Vector3>());
        c2.Points["Head"][0].Add(new Vector3(0.0f, 3.0f, 0.2f));   //head
        c2.Points["Head"][0].Add(new Vector3(0.0f, 3.5f, 0.2f));   //head
        c2.Points["Head"][0].Add(new Vector3(0.0f, 3.0f, 0.5f));   //head

        //arm
        c2.Points["Arm"].Add(new List<Vector3>());
        c2.Points["Arm"].Add(new List<Vector3>());
        c2.Points["Arm"][0].Add(new Vector3(0.75f, 1.0f, 0.0f));   //shoulder 1
        c2.Points["Arm"][1].Add(new Vector3(-0.75f, 1.0f, 0.0f));  //shoulder 2
        c2.Points["Arm"][0].Add(new Vector3(1.25f, 0.75f, 0.25f));   //elbo 1
        c2.Points["Arm"][1].Add(new Vector3(-1.25f, 0.75f, 0.25f));  //elbo 2
        c2.Points["Arm"][0].Add(new Vector3(1.5f, 0.5f, 0.5f));   //hand 1
        c2.Points["Arm"][1].Add(new Vector3(-1.5f, 0.5f, 0.5f));  //hand 2

        //Leg1
        c2.Points["Leg"].Add(new List<Vector3>());
        c2.Points["Leg"].Add(new List<Vector3>());
        c2.Points["Leg"][0].Add(new Vector3(-0.5f, 0.0f, 0.0f));  //hip  front 1
        c2.Points["Leg"][1].Add(new Vector3(0.5f, 0.0f, 0.0f));   //hip  front 2
        c2.Points["Leg"][0].Add(new Vector3(-1.0f, -1.0f, 0.0f));  //knee front 1
        c2.Points["Leg"][1].Add(new Vector3(1.0f, -1.0f, 0.0f));  //knee front 2
        c2.Points["Leg"][0].Add(new Vector3(-1.1f, -2.0f, 0.0f));  //foot front 1
        c2.Points["Leg"][1].Add(new Vector3(1.25f, -2.0f, 0.0f));  //foot front 2

        //tail
        c2.Points["Tail"].Add(new List<Vector3>());
        c2.Points["Tail"][0].Add(new Vector3(0.0f, 0.0f, -0.25f));
        c2.Points["Tail"][0].Add(new Vector3(0.0f, 0.5f, -1.0f));
        c2.Points["Tail"][0].Add(new Vector3(0.0f, 1.0f, -1.5f));
        c2.Points["Tail"][0].Add(new Vector3(0.0f, 1.5f, -1.75f));
        c2.Points["Tail"][0].Add(new Vector3(0.0f, 2.0f, -1.7f));
        c2.Points["Tail"][0].Add(new Vector3(0.0f, 2.5f, -1.5f));
    }

    //Add points for Creature 3
    private void Creature3()
    {
        c3 = new Creature();
        //start point
        c3.Start = new Vector3(0.0f, 0.0f, 0.0f);

        //neck
        c3.Points["Neck"].Add(new List<Vector3>());
        c3.Points["Neck"][0].Add(new Vector3(0.0f, 0.5f, 0.0f));   //neck
        c3.Points["Neck"][0].Add(new Vector3(0.0f, 0.75f, -0.1f));   //neck
        c3.Points["Neck"][0].Add(new Vector3(0.0f, 1.0f, -0.2f));   //neck

        //head
        c3.Points["Head"].Add(new List<Vector3>());
        c3.Points["Head"][0].Add(new Vector3(0.0f, 1.0f, 0.2f));   //head
        c3.Points["Head"][0].Add(new Vector3(0.0f, 1.5f, 0.2f));   //head
        c3.Points["Head"][0].Add(new Vector3(0.0f, 1.0f, 0.5f));   //head
        c3.Points["Head"][0].Add(new Vector3(0.0f, 1.0f, 1.0f));   //head

        //Leg1
        c3.Points["Leg"].Add(new List<Vector3>());
        c3.Points["Leg"].Add(new List<Vector3>());
        c3.Points["Leg"][0].Add(new Vector3(-0.5f, 0.0f, 0.0f));  //hip  front 1
        c3.Points["Leg"][1].Add(new Vector3(0.5f, 0.0f, 0.0f));   //hip  front 2
        c3.Points["Leg"][0].Add(new Vector3(-1.0f, -1.0f, 0.0f));  //knee front 1
        c3.Points["Leg"][1].Add(new Vector3(1.0f, -1.0f, 0.0f));  //knee front 2
        c3.Points["Leg"][0].Add(new Vector3(-1.1f, -2.0f, 0.0f));  //foot front 1
        c3.Points["Leg"][1].Add(new Vector3(1.25f, -2.0f, 0.0f));  //foot front 2

        //Legs 2
        c3.Points["Leg"].Add(new List<Vector3>());
        c3.Points["Leg"].Add(new List<Vector3>());
        c3.Points["Spine"].Add(new List<Vector3>());
        c3.Points["Spine"][0].Add(new Vector3(0.0f, -0.2f, -0.5f));   //spine
        c3.Points["Spine"][0].Add(new Vector3(0.0f, -0.4f, -1.5f));   //spine
        c3.Points["Spine"][0].Add(new Vector3(0.0f, -0.2f, -2.5f));   //spine
        c3.Points["Leg"][2].Add(new Vector3(-0.5f, -0.25f, -2.5f));   //hip  front 2
        c3.Points["Leg"][3].Add(new Vector3(0.5f, -0.25f, -2.5f));   //hip  front 2
        c3.Points["Leg"][2].Add(new Vector3(-1.0f, -1.0f, -2.5f));  //knee front 1
        c3.Points["Leg"][3].Add(new Vector3(1.0f, -1.0f, -2.5f));  //knee front 2
        c3.Points["Leg"][2].Add(new Vector3(-1.25f, -2.0f, -2.5f));  //foot front 1
        c3.Points["Leg"][3].Add(new Vector3(1.1f, -2.0f, -2.5f));  //foot front 2

        //tail
        c3.Points["Tail"].Add(new List<Vector3>());
        c3.Points["Tail"][0].Add(new Vector3(0.0f, 0.5f, -3.0f));
    }

    // Use this for initialization
    void Start()
    {
        Creature1();
        Creature2();
        Creature3();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
        {
            //Generate Creature1
            g.GetComponent<Metaballs>().Generate(c2);
        }
    }

    private void Update()
    {
        //Choose creature to generate
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
        //    {
        //        //Generate Creature1
        //        g.GetComponent<Metaballs>().Generate(c1);
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
        //    {
        //        //Generate Creature1
        //        g.GetComponent<Metaballs>().Generate(c2);
        //    }
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
        //    {
        //        //Generate Creature1
        //        g.GetComponent<Metaballs>().Generate(c3);
        //    }
        //}

        if (Input.GetKey(KeyCode.UpArrow))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
            {
                g.transform.Rotate(Vector3.left * turnSpeed * Time.deltaTime, Space.World);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
            {
                g.transform.Rotate(Vector3.right * turnSpeed * Time.deltaTime, Space.World);
            }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
            {
                g.transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime, Space.World);
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
            {
                g.transform.Rotate(Vector3.down * turnSpeed * Time.deltaTime, Space.World);
            }
        }

    }
}
