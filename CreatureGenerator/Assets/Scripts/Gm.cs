using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gm : MonoBehaviour
{
    [SerializeField]
    private GameObject camera;
    //Creatures
    private Creature c1 = new Creature();

    private float turnSpeed = 20;

    //UI fields
    Slider HeadsSlider;
    Dropdown HeadType;
    Slider NeckSlider;
    Slider TorsoSlider;
    Dropdown TorsoDropdown;
    Slider ArmSlider;
    Dropdown ArmDropdown;
    Slider LegSlider;
    Dropdown LegSize;
    Dropdown LegType;
    Slider TailSlider;
    Slider TailLengthSlider;
    Dropdown TailType;
    Slider DetailSlider;

    // Use this for initialization
    void Start()
    {
        HeadsSlider = GameObject.Find("Heads_sldr").GetComponent<Slider>();
        HeadType = GameObject.Find("HeadType_drpdwn").GetComponent<Dropdown>();
        NeckSlider = GameObject.Find("neckLength_sldr").GetComponent<Slider>();
        TorsoSlider = GameObject.Find("torsoSize_slider").GetComponent<Slider>();
        TorsoDropdown = GameObject.Find("torso_drpdwn").GetComponent<Dropdown>();
        ArmSlider = GameObject.Find("Arm_slider").GetComponent<Slider>();
        ArmDropdown = GameObject.Find("Arm_drpdwn").GetComponent<Dropdown>();
        LegSlider = GameObject.Find("Legs_sldr").GetComponent<Slider>();
        LegSize = GameObject.Find("LegSize_drpdwn").GetComponent<Dropdown>();
        LegType = GameObject.Find("LegType_drpdwn").GetComponent<Dropdown>();
        TailSlider = GameObject.Find("Tail_sldr").GetComponent<Slider>();
        TailLengthSlider = GameObject.Find("TailLength_sldr").GetComponent<Slider>();
        TailType = GameObject.Find("Tail_drpdwn").GetComponent<Dropdown>();
        DetailSlider = GameObject.Find("detail_sldr").GetComponent<Slider>();
        RandomCreature();
    }

    public void RandomCreature()
    {
        c1.Randomise();
        GenerateCreature();
        HeadsSlider.value = c1.Head;
        HeadType.value = (int)c1.HeadType;
        NeckSlider.value = c1.neckLength;
        TorsoSlider.value = c1.TorsoSize;
        TorsoDropdown.value = (int)c1.TorsoType;
        ArmSlider.value = c1.ArmPairs;
        ArmDropdown.value = (int)c1.ArmSize;
        LegSlider.value = c1.LegPairs;
        LegSize.value = (int)c1.LegSize;
        LegType.value = (int)c1.LegType;
        TailSlider.value = c1.Tail;
        TailLengthSlider.value = c1.TailLength;
        TailType.value = (int)c1.TailType;
    }

    public void GenerateButton()
    {
        c1.Head = (int)HeadsSlider.value;
        c1.HeadType = (Creature.TypeHead)HeadType.value;
        c1.neckLength = (int)NeckSlider.value;
        c1.TorsoSize = (int)TorsoSlider.value;
        c1.TorsoType = (Creature.TypeTorso)TorsoDropdown.value;
        c1.ArmPairs = (int)ArmSlider.value;
        c1.ArmSize = (Creature.Size)ArmDropdown.value;
        c1.LegPairs = (int)LegSlider.value;
        c1.LegSize = (Creature.Size)LegSize.value;
        c1.LegType = (Creature.Type)LegType.value;
        c1.Tail = (int)TailSlider.value;
        c1.TailLength = (int)TailLengthSlider.value;
        c1.TailType = (Creature.TypeTail)TailType.value;

        GenerateCreature();
    }

    void GenerateCreature()
    {
        //Generate Creature1
        c1.Generate();
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
        {
            g.GetComponent<Metaballs>().Generate(c1);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
            {
                g.transform.Rotate(Vector3.left * turnSpeed * Time.deltaTime, Space.World);
            }
        }
        if (Input.GetKey(KeyCode.K))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
            {
                g.transform.Rotate(Vector3.right * turnSpeed * Time.deltaTime, Space.World);
            }
        }
        if (Input.GetKey(KeyCode.J))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("creature"))
            {
                g.transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime, Space.World);
            }
        }
        if (Input.GetKey(KeyCode.L))
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

    public void DetailChange()
    {
        Metaballs.detail = DetailSlider.value;
    }
}
