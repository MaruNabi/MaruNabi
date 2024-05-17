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
    [SerializeField] private Animator _animator;
    SpriteRenderer spriteRenderer;
    private Sequence sequence2;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        
        zoomCamera.gameObject.SetActive(true);
        
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(spriteRenderer.DOFade(1, 1f))
            .Join(transform.DOMoveY(transform.position.y+2f, 1f))
            .Append(transform.DOMove(target.position, 6f))
            .JoinCallback(() =>
            {
                sequence2 = DOTween.Sequence();
                sequence2
                    .Append(transform.DOMoveY(transform.position.y - 2f, 1))
                    .Append(transform.DOMoveY(transform.position.y + 2f, 1))
                    .Append(transform.DOMoveY(transform.position.y - 2f, 1))
                    .AppendCallback(() => sequence.Kill())
                    .Join(transform.DOMove(target.position, 2f))
                    .AppendCallback(() =>
                    {
                        stage3Camera.Priority = 100;
                        stage3Camera.gameObject.SetActive(true);
                        
                    })
                    .AppendInterval(1f)
                    .OnComplete(() => _animator.enabled= true);
            });
    }
}
