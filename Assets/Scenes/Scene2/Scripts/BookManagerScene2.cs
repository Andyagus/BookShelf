using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BookManagerScene2 : MonoBehaviour
{
    // public string bookName;
    // public bool bookSelected;
    public float placementOffset;
    // Update is called once per frame
    
    public Canvas smallCanvas;
    public float bookCardOffset;

    public List<Sprite> bookCoverSprites;
    
    public Slider slider;
    public List<TransformDot> sliderTransformDots;

    public Image slider1Handle;
    
    public List<BookSpecificScene2> books;

    public Action<TransformDot> onBookChanged;


    private ButtonManager _buttonManager;

    private List<Image> sliderImages;

    private List<RayInteractor> _rayInteractors;
    
    private void Awake()
    {
        AssignPointValues();
        AssignBookCovers();

        sliderImages = slider.GetComponentsInChildren<Image>().ToList();

        FadeOutSlider();
        
        _buttonManager = FindObjectOfType<ButtonManager>();
        _buttonManager.onPokeButtonPressed += OnPokeButtonPressed;
    }


    private void Start()
    {
        _rayInteractors = FindObjectsOfType<RayInteractor>(includeInactive:true).ToList();
        Debug.Log(_rayInteractors.Count);

    }

    
    public void DisableRayInteractors()
    {
        
        foreach (var interactor in _rayInteractors)
        {
            interactor.gameObject.SetActive(false);
        }
    }
    
    private void FadeOutSlider()
    {
        slider.gameObject.SetActive(false);
        foreach (var image in sliderImages)
        {
            image.DOFade(0, 0f);
        }
    }

    private void OnPokeButtonPressed()
    {
        slider.gameObject.SetActive(true);
        foreach (var image in sliderImages)
        {
            image.DOFade(1, 1f);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            var currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
        }
    }

    private bool enlarged = false;
    public void AnimateUpHandle()
    {
        if (!enlarged)
        {
            var currentTransform = slider1Handle.transform;
            slider1Handle.transform.DOScale(currentTransform.localScale * 1.4f, 0.85f).SetEase(Ease.InElastic);
            enlarged = true; 
        }
    }

    private void AssignPointValues()
    {
        int n = sliderTransformDots.Count;

        for (int i = 0; i < n; i++)
        {
            float point = (float)i / (n - 1);
            sliderTransformDots[i].pointValue = point;
            books[i].bookdot = sliderTransformDots[i];
        }
    }
    
    private void AssignBookCovers()
    {
        int n = books.Count;

        for (int i = 0; i < n; i++)
        {
            books[i].bookCover = bookCoverSprites[i];
        }
    }

    public void OnSliderValueChanged(float value)
    {

        var workingDot = GetClosestValue(value);

        if (slider.value != workingDot.pointValue)
        {
            slider.value = workingDot.pointValue + placementOffset;
            onBookChanged(workingDot);
        }

       
        
    }
    
    public TransformDot GetClosestValue(float value)
    {
        var closestPointValue = 0f;
        var minDifference = Mathf.Abs(value - closestPointValue);
        var workingDot = sliderTransformDots[0];
        foreach (var dot in sliderTransformDots)
        {
            var pointValue = dot.pointValue;
            
            var difference = Mathf.Abs(value - pointValue);
        
            if (difference < minDifference)
            {
                closestPointValue = pointValue;
                minDifference = difference;
                workingDot = dot;
            }
        
        }
        
        return workingDot;
    }


    
}
