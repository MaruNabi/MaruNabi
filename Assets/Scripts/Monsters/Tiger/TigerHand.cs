using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TigerHand : MonoBehaviour
{
    [SerializeField] private bool isRightHand;
    [SerializeField] private Transform sideHandTransform;
    [SerializeField] private GameObject sideHandPrefab;
    [SerializeField] private Transform digHandTransform;
    [SerializeField] private GameObject digHandPrefab;

    private Animator handAnimator;
    private SpriteRenderer handSpriteRenderer;
    
    private void Start()
    {
        handAnimator = GetComponent<Animator>();
        handSpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void ExitAnimation()
    {
        handAnimator.SetTrigger("Exit");
        handSpriteRenderer.DOFade(0, 0.5f);
    }
    
    public void EnterAnimation()
    {
        handAnimator.SetTrigger("Enter");
        handSpriteRenderer.DOFade(1, 0.5f);
    }
    
    public async UniTaskVoid SideHandAttack()
    {
        ExitAnimation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        
        var sideHand = Instantiate(sideHandPrefab, sideHandTransform);
        sideHand.transform.position = sideHandTransform.position;
        sideHand.GetComponent<TigerSideAtk>().Move(isRightHand);
        
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        EnterAnimation();
    }
    
    public async UniTaskVoid DigHandAttack()
    {
        ExitAnimation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f));
        
        var digHand = Instantiate(digHandPrefab, digHandTransform);
        digHand.transform.position = digHandTransform.position;
        digHand.GetComponent<TigerDigAtk>().Move(isRightHand, Random.Range(1, 4));
        
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        EnterAnimation();
    }
}