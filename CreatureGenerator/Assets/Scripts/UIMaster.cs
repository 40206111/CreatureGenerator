using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMaster : MonoBehaviour
{
    [SerializeField]
    Text theText;
        public void OnValueChanged()
    {
        theText.text = "" + GetComponent<Slider>().value;
    }

}
