using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gm : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;
    //Creatures
    private Creature c1 = new Creature();

    private float turnSpeed = 20;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
        {
            //Generate Creature1
            c1.Generate();
            g.GetComponent<Metaballs>().Generate(c1);
        }
    }

    private void Update()
    {
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

        Vector3 newCamPos = camera.transform.position;
        if (Input.GetKey(KeyCode.W))
        {
              newCamPos.z -= 10.0f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newCamPos.z += 10.0f * Time.deltaTime;
        }
        camera.transform.position = newCamPos;
    }
}
