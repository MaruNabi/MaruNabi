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

    SpriteRenderer spriteRenderer;
    private Sequence sequence2;

    public void StartProduction()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        zoomCamera.gameObject.SetActive(true);

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
                    .AppendCallback(() => { stage3Camera.Priority = 21; })
                    .Append(transform.DOMoveY(transform.position.y - 2f, 1))
                    .Append(transform.DOMoveY(transform.position.y + 2f, 1))
                    .AppendCallback(() => sequence.Kill())
                    .Join(transform.DOMove(target.position, 1f))
                    .Join(spriteRenderer.DOFade(0,1f))
                    .AppendInterval(1f);
            });
    }
    
    public void ClearProduction()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(spriteRenderer.DOFade(1, 1f))
            .Join(transform.DOMoveY(transform.position.y + 10f, 3f));
    }
}