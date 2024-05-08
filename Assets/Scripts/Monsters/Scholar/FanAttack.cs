using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class FanAttack : MonoBehaviour
{
    private Sequence sequence;
    
    private void Start()
    {
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(60f, 2f))
            .OnComplete(() => Destroy(gameObject));
    }
    
    void OnDestroy()
    {
        sequence.Kill();
    }
}