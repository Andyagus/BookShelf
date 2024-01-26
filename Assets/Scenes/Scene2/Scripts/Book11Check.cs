using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Book11Check : MonoBehaviour
{
    public SpriteRenderer check;

    private Vector3 originalCheckScale;

    private void Awake()
    {
        originalCheckScale = check.transform.localScale;
    }

    private void Start()
    {
        
        OnBookSelect();
    }

    public void OnBookSelect()
    {
        check.DOFade(0, 0f);
        check.transform.DOScale(0, 0f);
    }
    
    public void OnBookRelease()
    {
        check.DOFade(1, 0.65f);
        check.transform.DOScale(originalCheckScale, 0.85f).SetEase(Ease.InOutElastic);
    }
}
