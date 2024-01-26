using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class JoystickManager : MonoBehaviour
{
    
    private SmallCanvas _smallCanvas;

    [SerializeField] private GameObject slider2Canvas; 
    
    [SerializeField] private Slider slider1;
    [SerializeField] private Slider slider2;
    
    [SerializeField] private GameObject slider1Handle;
    [SerializeField] private GameObject slider2Handle;

    [SerializeField] private Transform handleSlideArea2Transform;
    
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;

    [SerializeField] private GameObject surface;
    
    private void Awake()
    {
        _smallCanvas = FindObjectOfType<SmallCanvas>();
        _smallCanvas.onSliderHeld += OnSliderHeld;
    }

    private bool handleRepositioned;
    private void Update()
    {

        if (!handleRepositioned)
        {
            var newPos = new Vector3(slider1Handle.transform.position.x, 
                slider2Canvas.transform.position.y, slider1Handle.transform.position.z);
            slider2Canvas.transform.position = newPos;
            slider2Canvas.transform.DOLocalMoveZ(yOffset, 0);
        }

        // if (!handleRepositioned)
        // {
        //     slider2.transform.position = slider1Handle.transform.position;
        //     surface.transform.position = slider2.transform.position;
        // }
        // var localPos = new Vector3(slider1Handle.transform.position.x, 
        //     slider1Handle.transform.position.y, slider1Handle.transform.position.z - zOffset);
        // slider2.transform.localPosition = localPos;
        //
        // var worldPos =  new Vector3(slider1Handle.transform.position.x, 
        //     slider1Handle.transform.position.y, slider2.transform.position.z);
        // slider2.transform.position = worldPos;
    }

    private void OnSliderHeld()
    {
        slider1Handle.transform.SetParent(handleSlideArea2Transform);
        slider2.handleRect = slider1Handle.GetComponent<RectTransform>();
        slider1Handle.transform.position = slider2Handle.transform.position;
        slider1Handle.GetComponent<RectTransform>().sizeDelta = slider2Handle.GetComponent<RectTransform>().sizeDelta;
        slider1.handleRect = null;
        handleRepositioned = true;
    }
    
}
