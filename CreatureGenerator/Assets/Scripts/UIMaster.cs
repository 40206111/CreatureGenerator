using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour
{
    Text theText;

    void Start()
    {
        theText = GetComponentInChildren<Text>();
    }

        public void OnValueChanged()
    {
        theText.text = ": " + GetComponent<Slider>().value;
    }

}
