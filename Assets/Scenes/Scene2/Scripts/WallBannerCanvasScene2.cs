using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WallBannerCanvasScene2 : MonoBehaviour
{
    
    //for editing 
    public Image bookImage;
    public TextMeshProUGUI title;
    public TextMeshProUGUI author;
    
    
    //for entrance animation
    public Slider progressSlider;

    public TextMeshProUGUI sliderPercentageText;
    public TextMeshProUGUI sliderPageText;
    
    public TextMeshProUGUI hours;
    public TextMeshProUGUI minutes;
    public TextMeshProUGUI pages;
    
    private SecondarySliderManagerScene2 _secondarySliderManager;

    public Image backgroundImage;
    private RectTransform backgroundImageRectTransform;
    private Vector3 backgroundImageOriginalTransform;

    // [SerializeField] private GameObject rayInteractorRight;
    // [SerializeField] private GameObject rayInteractorLeft;

    private List<RayInteractor> _rayInteractors;
    
    private void Awake()
    {
        
        _secondarySliderManager = GameObject.FindObjectOfType<SecondarySliderManagerScene2>();
        _secondarySliderManager.onSliderComplete += OnSliderComplete;

        backgroundImageRectTransform = backgroundImage.GetComponent<RectTransform>();
        backgroundImageOriginalTransform = backgroundImageRectTransform.localScale;
        
        InitiateAnimationSequence();
    }

    private void Start()
    {
        _rayInteractors = FindObjectsOfType<RayInteractor>(includeInactive:true).ToList();
    }

    private void EntranceAnimation()
    {

        EnableRayInteractors();
        
        progressSlider.DOValue(1, 3f);
        sliderPercentageText.DOText("44%", 4f, scrambleMode: ScrambleMode.Numerals);
        sliderPageText.DOText("108", 4f, scrambleMode: ScrambleMode.Numerals);
        
        
        hours.DOText("25", 4f, scrambleMode: ScrambleMode.Numerals);
        minutes.DOText("11", 4f, scrambleMode: ScrambleMode.Numerals);
        pages.DOText("105", 4f, scrambleMode: ScrambleMode.Numerals);
    }

    private void EnableRayInteractors()
    {
        foreach (var interactor in _rayInteractors)
        {
            interactor.gameObject.SetActive(true);
        }
    }
    
  
    
    private void InitiateAnimationSequence()
    {
        backgroundImage.DOFade(0, 0f);
        backgroundImageRectTransform.DOScale(0, 0f);
    }

    private void OnSliderComplete()
    {
        var sequence = DOTween.Sequence();
        sequence.AppendCallback(() => EntranceAnimation());
        sequence.Append(backgroundImage.DOFade(1, 1f));
        sequence.Join(backgroundImageRectTransform.DOScale(backgroundImageOriginalTransform, 1f));
    }
}
