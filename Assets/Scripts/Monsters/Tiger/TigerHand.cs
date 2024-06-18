using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using JetBrains.Annotations;
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
    [SerializeField] private Transform slapHandTransform;
    [SerializeField] private GameObject slapHandPrefab;
    [SerializeField] private Transform sideHandTransform2;
    [SerializeField] private GameObject sideHandPrefab2;
    private List<IDelete> currentHands;

    private Animator handAnimator;
    private SpriteRenderer handSpriteRenderer;


    private void Start()
    {
        handAnimator = GetComponent<Animator>();
        handSpriteRenderer = GetComponent<SpriteRenderer>();
        currentHands = new List<IDelete>();
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

    public async UniTaskVoid SideHandAttack(CancellationToken _token)
    {
        ExitAnimation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();

        var sideHand = Instantiate(sideHandPrefab, sideHandTransform);
        var delete = sideHand.GetComponent<IDelete>();
        currentHands.Add(delete);
        sideHand.transform.position = sideHandTransform.position;
        sideHand.GetComponent<TigerSideAtk>().Move(isRightHand);

        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();
        EnterAnimation();
        currentHands.Remove(delete);
    }

    public async UniTaskVoid SideHandAttack2(CancellationToken _token)
    {
        ExitAnimation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();

        var sideHand = Instantiate(sideHandPrefab2, sideHandTransform2);
        var delete = sideHand.GetComponent<IDelete>();
        currentHands.Add(delete);
        sideHand.transform.position = sideHandTransform2.position;
        sideHand.GetComponent<TigerSideAtk2>().Move(isRightHand);

        await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();
        EnterAnimation();
        currentHands.Remove(delete);
    }

    public async UniTaskVoid DigHandAttack(CancellationToken _token)
    {
        ExitAnimation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();

        var digHand = Instantiate(digHandPrefab, digHandTransform);
        var delete = digHand.GetComponent<IDelete>();
        currentHands.Add(delete);

        digHand.transform.position = digHandTransform.position;
        digHand.GetComponent<TigerDiagonalAtk>().Move(isRightHand, Random.Range(1, 4));

        await UniTask.Delay(TimeSpan.FromSeconds(2.5f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();
        EnterAnimation();
        currentHands.Remove(delete);
    }

    public async UniTaskVoid SlapHandAttack(CancellationToken _token)
    {
        ExitAnimation();
        await UniTask.Delay(TimeSpan.FromSeconds(0.75f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();

        var slapHand = Instantiate(slapHandPrefab, slapHandTransform);
        var delete = slapHand.GetComponent<IDelete>();
        currentHands.Add(delete);
        slapHand.transform.position = slapHandTransform.position;
        slapHand.GetComponent<TigerSlapAtk>().Move(isRightHand);

        await UniTask.Delay(TimeSpan.FromSeconds(2.5f), cancellationToken: _token);
        _token.ThrowIfCancellationRequested();
        EnterAnimation();
        currentHands.Remove(delete);
    }

    public void DeleteHands()
    {
        Debug.Log("Delete Hands" + currentHands.Count);

        foreach (var hand in currentHands)
        {
            try
            {
                DOTween.Kill(hand);
                hand?.Delete();
            }
            catch (Exception e)
            {
            }
        }

        EnterAnimation();
    }
}