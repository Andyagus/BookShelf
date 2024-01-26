using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SecondarySliderManagerScene2 : MonoBehaviour
{

    private SmallCanvas smallCanvas;
    public Slider slider1;
    public GameObject slider1Handle;
    public RectTransform slider1HandleRect;

    public Slider slider2;
    public RectTransform slider2HandleRect;
    public Transform slider2HandleSlideArea;

    private bool switchedSlider;

    public Image backgroundStrokeIndicator; 
    public Image foregroundStrokeIndicator;

    public RectTransform slider1TargetRect;
    
    public Action onSliderComplete;
    
    private void Awake()
    {
        smallCanvas = GameObject.Find("BookInformationCanvas").GetComponent<SmallCanvas>();
        smallCanvas.onSliderHeld += OnSliderHeld;

        backgroundStrokeIndicator.DOFade(0, 0);

    }

    private bool slider2Filled; 
    private void Update()
    {
        if (!switchedSlider)
        {
            var newLocaPosition = new Vector3(slider1Handle.transform.localPosition.x, slider2.transform.localPosition.y, slider1.transform.localPosition.z);
            slider2.transform.localPosition = newLocaPosition;
        }

        if (!slider2Filled)
        {
            if (slider2.value >= 0.8f)
            {
                slider1.GetComponent<RectTransform>().DOSizeDelta(slider1TargetRect.sizeDelta, 2.5f);
                slider1.GetComponentInChildren<Image>().DOFade(0, 2f);
            }
            if (slider2.value >= 0.99f)
            {
                var sequence = DOTween.Sequence();
                sequence.AppendCallback(() => slider2Filled = true);
                sequence.Append(foregroundStrokeIndicator.DOFillAmount(1, 0.25f));
                sequence.AppendCallback(() => onSliderComplete?.Invoke());
                sequence.Append(foregroundStrokeIndicator.DOFade(0, 0.3f));
                sequence.Join(slider1.gameObject.transform.DOScale(0, 0.3f));
                sequence.Join(backgroundStrokeIndicator.DOFade(0, 0.3f));
                sequence.Join(slider1Handle.GetComponent<Image>().DOFade(0, 0.3f));
            }
        }
    }

    private void OnSliderHeld()
    {
        AlignAndSetSlider();
        DisplayIndicator();
    }

    private void DisplayIndicator()
    {
        backgroundStrokeIndicator.DOFade(0.25f, 2f);
    }

    private void AlignAndSetSlider()
    {
        slider1Handle.transform.SetParent(slider2HandleSlideArea);
        slider2.handleRect = slider1HandleRect;
        slider1HandleRect.sizeDelta = slider2HandleRect.sizeDelta;
        slider1HandleRect.localPosition = slider2HandleRect.localPosition;
        slider1HandleRect.localRotation = slider2HandleRect.localRotation;
        slider1.handleRect = null;
        slider1.fillRect = null;
        switchedSlider = true;    
    }
}
