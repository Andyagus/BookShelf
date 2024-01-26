using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Oculus.Interaction;
using OVR.OpenVR;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmallCanvas : MonoBehaviour
{
    private BookManagerScene2 bookManager;
    
    
    public bool active;
    public bool expanded;
    private Canvas smallCanvas;
    public Image background;
    public Image bookMask;
    public Image bookCover;

    public GameObject textContainer;
    private List<TextMeshProUGUI> bookInformationTexts;
    private Image progressBarImageInText;
    
    
    private Vector3 originalScale;

    private Vector3 originalPos;
    public Transform targetPos;

    private float originalYPos;
    private float targetYPos;
    
    public RectTransform targetRectTransform;

    public Action onSliderHeld;
    
    public SecondarySliderManagerScene2 secondarySliderManagerScene2;

    public bool secondarySliderActive;
    
    private void Awake()
    {
        DOTween.SetTweensCapacity(500,125);
        SliderHeldTimer();
        
        bookManager = FindObjectOfType<BookManagerScene2>();
        bookManager.onBookChanged += OnBookChanged;

        secondarySliderManagerScene2 = FindObjectOfType<SecondarySliderManagerScene2>();
        secondarySliderManagerScene2.onSliderComplete += OnSliderComplete;
        
        smallCanvas = GetComponent<Canvas>();
        originalScale = transform.localScale;
        originalPos = transform.position;

        originalYPos = transform.position.z;
        targetYPos = targetPos.position.z;
        
        var targetScale = new Vector3(0, 0, 0);
        smallCanvas.transform.DOScale(targetScale, 0f);
        // smallCanvas.transform.DOMove(targetPos.position, 0f);
        background.DOFade(0, 0f);
        bookMask.DOFade(0, 0f);
        bookCover.DOFade(0, 0f);


        bookInformationTexts = textContainer.GetComponentsInChildren<TextMeshProUGUI>().ToList();
        progressBarImageInText = textContainer.GetComponentInChildren<Image>();


        foreach (var text in bookInformationTexts)
        {
            text.DOFade(0, 0f);
        }

        progressBarImageInText.DOFade(0, 0f);
    }
    
    private void OnBookChanged(TransformDot dot)
    {
        if (!active)
        {
            DOTween.Kill("HideBook");
            var sequence = DOTween.Sequence().SetId("ShowBook");
            var pos = new Vector3(smallCanvas.transform.position.x, originalYPos, smallCanvas.transform.position.z);
            // sequence.Append(smallCanvas.transform.DOMove(pos, 1f));
            sequence.AppendCallback(() => TimerController(false));
            sequence.Join(smallCanvas.transform.DOScale(originalScale, 1f));
            sequence.Join(background.DOFade(1f, 1f));
            sequence.Join(bookMask.DOFade(1f, 1f));
            sequence.Join(bookCover.DOFade(1f, 1f));
            sequence.AppendCallback(() => active = true);
            
            //Fix this fade out thing
            // sequence.AppendInterval(2);
            // sequence.AppendCallback(HideBook);
        }
    }

    private void HideBook()
    {
        DOTween.Kill("ShowBook");
        var pos = new Vector3(smallCanvas.transform.position.x, smallCanvas.transform.position.y - 0.2f, smallCanvas.transform.position.z);
        var sequence = DOTween.Sequence().SetId("HideBook");
        var targetScale = new Vector3(0, 0, 0);   
        // sequence.Append(smallCanvas.transform.DOMove(pos, 1f));
        sequence.Join(smallCanvas.transform.DOScale(targetScale, 1f));
        sequence.Join(background.DOFade(0f, 1f));
        sequence.Join(bookMask.DOFade(0f, 1f));
        sequence.Join(bookCover.DOFade(0f, 1f));
        sequence.AppendCallback(() => active = false);
    }


    public void SliderHeldTimer()
    {
        var sequence = DOTween.Sequence().SetId("SliderTimer").SetAutoKill(false);
        for (var i = 0; i < 1.5; i++)
        {
            sequence.AppendInterval(0.75f);
        }
        sequence.AppendCallback(() => Debug.Log("Timer Complete"));
        sequence.AppendCallback(() => ChangeBackgroundSize());
        sequence.AppendCallback(() => onSliderHeld());
        sequence.AppendCallback(() => secondarySliderActive = true);

        sequence.Pause();
    }

    private void ChangeBackgroundSize()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(background.GetComponent<RectTransform>().DOSizeDelta(targetRectTransform.sizeDelta, 1));
        foreach (var text in bookInformationTexts)
        {
            sequence.Join(text.DOFade(1, 1f));
        }

        sequence.Join(progressBarImageInText.DOFade(1, 1f));
    }
    

    public void TimerController(bool state)
    {
        if (!secondarySliderActive)
        {
            if (state)
            {
                Debug.Log("Timer Started");
                DOTween.Restart("SliderTimer");
            }
            else
           
            {
                Debug.Log("Timer Paused");
                DOTween.Pause("SliderTimer");
            }
        }
    }
    
    public void SliderSelected(bool state, PointerEvent eventData)
    {
        if (state)
        {
            
        }else if (!state)
        {
            Debug.Log("SLIDER IS DESELECTED");
        }
    }

    private void OnSliderComplete()
    {
        // var tempImages = new List<Image>
        // {
        //     background,
        //     bookMask,
        //     bookCover,
        //     progressBarImageInText
        // };
        //
        var images = GetComponentsInChildren<Image>(false);
        foreach (var image in images)
        {
            image.DOFade(0, 0.2f);
        }

        foreach (var text in bookInformationTexts)
        {
            text.DOFade(0, 0.2f);
        }
    }
    
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     if (active)
        //     {
        //         var pos = new Vector3(smallCanvas.transform.position.x, smallCanvas.transform.position.y - 0.2f, smallCanvas.transform.position.z);
        //         var sequence = DOTween.Sequence();
        //         var targetScale = new Vector3(0, 0, 0);   
        //         // sequence.Append(smallCanvas.transform.DOMove(pos, 1f));
        //         sequence.Join(smallCanvas.transform.DOScale(targetScale, 1f));
        //         sequence.Join(background.DOFade(0f, 1f));
        //         sequence.Join(bookMask.DOFade(0f, 1f));
        //         sequence.Join(bookCover.DOFade(0f, 1f));
        //         sequence.AppendCallback(() => active = false);
        //     }
        // }   
    }
}
