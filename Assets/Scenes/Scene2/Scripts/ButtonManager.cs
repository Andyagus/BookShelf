using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{

    public float fadeOutTime;
    public GameObject glow;
    public Transform pokeButtonTransform;
    // public Image bookImage;
    
    public Action onPokeButtonPressed;

    private List<Image> buttonImages;

    public PokeInteractable buttonPokeInteractable;

    public GameObject buttonVisual;
    public GameObject buttonBackstop;

    private void Awake()
    {
        buttonImages = pokeButtonTransform.GetComponentsInChildren<Image>().ToList();
    }

    public void ButtonPressed()
    {
        Debug.LogWarning("PRESSED BUTTON");
        buttonPokeInteractable.enabled = false;
        buttonVisual.transform.position = buttonBackstop.transform.position;
        foreach (var image in buttonImages)
        {
            image.DOFade(0, fadeOutTime-fadeOutTime/2f);
        }

        pokeButtonTransform.DOScale(0, fadeOutTime);

        var s = DOTween.Sequence();
        s.Append(glow.transform.DOScale(0.007f, 0.6f));
        s.Join(glow.GetComponent<SpriteRenderer>().DOFade(0, 1));
        
        
        
        onPokeButtonPressed();
    }
}