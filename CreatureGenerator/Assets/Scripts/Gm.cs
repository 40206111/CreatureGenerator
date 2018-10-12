using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gm : MonoBehaviour {

    private Creature c1;
    private GameObject creature2;
    private GameObject creature3;

    private void Creature1()
    {
        c1 = new Creature();
        c1.points.Add(new Vector3(0.0f, 0.0f, 0.0f));
        c1.points.Add(new Vector3(0.0f, 1.0f, 0.0f));
    }

    // Use this for initialization
    void Start () {
        Creature1();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        //check if points exsist
        if (c1 == null)
        {
            return;
        }
        //change gizmo colour to black
        Gizmos.color = Color.cyan;
        //draw points of creature
        for (int i = 0; i < c1.points.Count; i++)
        {
            Gizmos.DrawSphere(c1.points[i], 0.1f);
        }
    }
}
