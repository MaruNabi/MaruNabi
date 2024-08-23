using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Yarn;

public class Wisp : MonoBehaviour
{
    private Vector3 originalPosition;
    public float duration = 2.5f;

    private void Start()
    {
        originalPosition = transform.position;
        
    }

    private void OnEnable()
    {
        transform.DOMoveY(transform.position.y+0.5f, duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
    
    private void OnDisable()
    {
        transform.DOKill();
        transform.position = originalPosition;
    }
}
