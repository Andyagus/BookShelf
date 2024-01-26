using System.Collections;
using System.Collections.Generic;
using DG.Tweening;using UnityEngine;

public class RotateLogo : MonoBehaviour
{
    public GameObject bezierCircle;
    // Start is called before the first frame update
    void Start()
    {
        bezierCircle.transform.DORotate(new Vector3(0, 360, 0), 2).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
