using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gm : MonoBehaviour {

    private Creature c1;

    private void Creature1()
    {
        c1 = new Creature();
        //start point (crotch)
        c1.points.Add(new Vector3(0.0f, 0.0f, 0.0f));
        c1.points.Add(new Vector3(0.0f, 1.0f, 0.0f));   //mid torso
        c1.points.Add(new Vector3(0.0f, 2.0f, 0.0f));   //top torso
        c1.points.Add(new Vector3(0.0f, 2.5f, 0.0f));   //neck
        c1.points.Add(new Vector3(0.0f, 3.0f, 0.2f));   //head
        c1.points.Add(new Vector3(1.0f, 2.0f, 0.0f));   //shoulder 1
        c1.points.Add(new Vector3(-1.0f, 2.0f, 0.0f));  //shoulder 2
        c1.points.Add(new Vector3(1.5f, 1.0f, 0.25f));   //elbo 1
        c1.points.Add(new Vector3(-1.5f, 1.0f, 0.25f));  //elbo 2
        c1.points.Add(new Vector3(2.0f, 0.0f, 0.5f));   //hand 1
        c1.points.Add(new Vector3(-2.0f, 0.0f, 0.5f));  //hand 2
        c1.points.Add(new Vector3(-1.0f, 0.0f, 0.0f));  //hip  front 1
        c1.points.Add(new Vector3(1.0f, 0.0f, 0.0f));   //hip  front 2
        c1.points.Add(new Vector3(-1.25f, -1.0f, 0.0f));  //knee front 1
        c1.points.Add(new Vector3(1.25f, -1.0f, 0.0f));  //knee front 2
        c1.points.Add(new Vector3(-1.5f, -2.0f, 0.0f));  //foot front 1
        c1.points.Add(new Vector3(1.5f, -2.0f, 0.0f));  //foot front 2

        //Legs 2
        c1.points.Add(new Vector3(0.0f, 0.0f, -1.0f));   //spine
        c1.points.Add(new Vector3(-1.0f, 0.0f, -1.0f));  //hip  front 1
        c1.points.Add(new Vector3(1.0f, 0.0f, -1.0f));   //hip  front 2
        c1.points.Add(new Vector3(-1.25f, -1.0f, -1.0f));  //knee front 1
        c1.points.Add(new Vector3(1.25f, -1.0f, -1.0f));  //knee front 2
        c1.points.Add(new Vector3(-1.5f, -2.0f, -1.0f));  //foot front 1
        c1.points.Add(new Vector3(1.5f, -2.0f, -1.0f));  //foot front 2

        //Legs 3
        c1.points.Add(new Vector3(0.0f, 0.0f, -2.0f));   //spine
        c1.points.Add(new Vector3(-1.0f, 0.0f, -2.0f));  //hip  front 1
        c1.points.Add(new Vector3(1.0f, 0.0f, -2.0f));   //hip  front 2
        c1.points.Add(new Vector3(-1.25f, -1.0f, -2.0f));  //knee front 1
        c1.points.Add(new Vector3(1.25f, -1.0f, -2.0f));  //knee front 2
        c1.points.Add(new Vector3(-1.5f, -2.0f, -2.0f));  //foot front 1
        c1.points.Add(new Vector3(1.5f, -2.0f, -2.0f));  //foot front 2

        //Legs 4
        c1.points.Add(new Vector3(0.0f, 0.0f, -3.0f));   //spine
        c1.points.Add(new Vector3(-1.0f, 0.0f, -3.0f));  //hip  front 1
        c1.points.Add(new Vector3(1.0f, 0.0f, -3.0f));   //hip  front 2
        c1.points.Add(new Vector3(-1.25f, -1.0f, -3.0f));  //knee front 1
        c1.points.Add(new Vector3(1.25f, -1.0f, -3.0f));  //knee front 2
        c1.points.Add(new Vector3(-1.5f, -2.0f, -3.0f));  //foot front 1
        c1.points.Add(new Vector3(1.5f, -2.0f, -3.0f));  //foot front 2

        //tail
        c1.points.Add(new Vector3(0.0f, 0.5f, -3.5f));
        c1.points.Add(new Vector3(0.0f, 1.0f, -4.0f));
    }

    // Use this for initialization
    void Start () {
        Creature1();
        //c1 = new Creature();
        //c1.points.Add(new Vector3(0.0f, 0.0f, 0.0f));
        //c1.points.Add(new Vector3(1.0f, 0.0f, 0.0f));
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
        {
            g.GetComponent<Metaballs>().Generate(c1);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        //// positive extremes (2, 3, 0.5)
        //// negative extremes (-2, -2, -4)
        Vector3 max = new Vector3(2.0f, 3.0f, 0.5f);
        Vector3 min = new Vector3(-2.0f, -2.0f, -4.0f);
        Vector3 range = max - min;

        //check if points exsist
        if (c1 == null)
        {
            return;
        }
        //draw points of creature
        for (int i = 0; i < c1.points.Count; i++)
        {
            Gizmos.color = new Color((c1.points[i].x - min.x) / range.x, (c1.points[i].y - min.y) / range.y, (c1.points[i].z - min.z) / range.z);
            Color temp = Gizmos.color;
            temp.a = 0.9f;
            Gizmos.color = temp;
            Gizmos.DrawSphere(c1.points[i], 0.1f);
        }
    }
}
