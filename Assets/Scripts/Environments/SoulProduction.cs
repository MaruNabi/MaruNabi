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
    [SerializeField] private GameObject soul;
    CinemachineConfiner2D confiner2D;
    SpriteRenderer spriteRenderer;
    private Sequence sequence2;
    
    List<ParticleSystem> particleSystems;

    private void Awake()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        particleSystems = new List<ParticleSystem>();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out ParticleSystem particleSystem))
            {
                particleSystems.Add(particleSystem);
            }
        }
    }

    public void StartProduction()
    {
        confiner2D = stage3Camera.GetComponent<CinemachineConfiner2D>();
        confiner2D.m_BoundingShape2D = previousCollider;
        
        Sequence sequence = DOTween.Sequence();

        sequence
            .AppendCallback(() =>
            {
                soul.SetActive(true);
            })
            .Join(soul.transform.DOMoveY(soul.transform.position.y + 5f, 1f))
            .AppendInterval(0.5f)
            .Append(soul.transform.DOMove(target.position, 6f))
            .JoinCallback(() =>
            {
                sequence2 = DOTween.Sequence();
                sequence2
                    .Append(soul.transform.DOMoveY(soul.transform.position.y - 2f, 1))
                    .Append(soul.transform.DOMoveY(soul.transform.position.y + 2f, 1))
                    .AppendCallback(() =>
                    {
                        confiner2D.m_BoundingShape2D = nextCollider;
                        stage3Camera.Priority = 21;
                    })
                    .Append(soul.transform.DOMoveY(soul.transform.position.y - 2f, 1))
                    .Append(soul.transform.DOMoveY(soul.transform.position.y + 2f, 1))
                    .AppendCallback(() => sequence.Kill())
                    .Join(soul.transform.DOMove(target.position, 1f))
                    .AppendInterval(1.5f)
                    .OnComplete(() =>
                    {
                        soul.SetActive(false);
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
            .Join(soul.transform.DOMoveY(soul.transform.position.y + 10f, 3f))
            .OnComplete(() =>
            {
                Fox.Stage3Clear?.Invoke(gameObject);
                Destroy(soul);
                Destroy(gameObject);
            });
    }
    
    public void ProductionSkip()
    {
        transform.position = target.position;
    }
}