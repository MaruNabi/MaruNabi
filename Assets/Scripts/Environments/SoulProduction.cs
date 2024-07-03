using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class SoulProduction : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] CinemachineVirtualCamera zoomCamera;
    [SerializeField] CinemachineVirtualCamera stage3Camera;

    [SerializeField] PolygonCollider2D previousCollider;
    [SerializeField] PolygonCollider2D nextCollider;
    
    CinemachineConfiner2D confiner2D;
    SpriteRenderer spriteRenderer;
    private Sequence sequence2;

    private void Awake()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
    }

    public void StartProduction()
    {
        confiner2D = stage3Camera.GetComponent<CinemachineConfiner2D>();
        confiner2D.m_BoundingShape2D = previousCollider;
        
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(spriteRenderer.DOFade(1, 1f))
            .Join(transform.DOMoveY(transform.position.y + 5f, 1f))
            .AppendInterval(0.5f)
            .Append(transform.DOMove(target.position, 6f))
            .JoinCallback(() =>
            {
                sequence2 = DOTween.Sequence();
                sequence2
                    .Append(transform.DOMoveY(transform.position.y - 2f, 1))
                    .Append(transform.DOMoveY(transform.position.y + 2f, 1))
                    .AppendCallback(() =>
                    {
                        confiner2D.m_BoundingShape2D = nextCollider;
                        //stage3Camera.Priority = 21;
                    })
                    .Append(transform.DOMoveY(transform.position.y - 2f, 1))
                    .Append(transform.DOMoveY(transform.position.y + 2f, 1))
                    .AppendCallback(() => sequence.Kill())
                    .Join(transform.DOMove(target.position, 1f))
                    .Join(spriteRenderer.DOFade(0,1f))
                    .AppendInterval(1f)
                    .OnComplete(() =>
                    {
                        Destroy(this);
                    });
            });
    }
    
    public void ClearProduction()
    {
        var color = spriteRenderer.color;
        color.a= 0;
        spriteRenderer.color = color;
        
        Sequence sequence = DOTween.Sequence();
        sequence
            .AppendInterval(1f)
            .Append(spriteRenderer.DOFade(1, 1f))
            .Join(transform.DOMoveY(transform.position.y + 10f, 3f))
            .OnComplete(() =>
            {
                Fox.Stage3Clear?.Invoke(gameObject);
                Destroy(gameObject);
            });
    }
    
    public void ProductionSkip()
    {
        transform.position = target.position;
    }
}