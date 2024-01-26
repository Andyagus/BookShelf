using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderManagerScene2Test : MonoBehaviour
{
    private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;
    
    // Start is called before the first frame update

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        valueText.text = slider.value.ToString();   
    }
}
