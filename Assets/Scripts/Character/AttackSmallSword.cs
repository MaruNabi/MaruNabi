using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using DG.Tweening;

public class AttackSmallSword : Bullet
{
    private Vector2 bulletPosition;
    private Vector3 targetVec;

    private void OnEnable()
    {
        SetBullet();

        
    }

    void Update()
    {
        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        NormalSwordMovement();
    }

    private void NormalSwordMovement()
    {
        if (lockedBulletVector.magnitude == 0)
        {
            Debug.Log("Hi");
            if (transform.rotation.y == 0)
            {
                //bulletPosition = transform.position;
                transform.DOMoveX(1, 1).SetLoops(-1, LoopType.Yoyo);
                //transform.DOMove(bulletPosition, 1);
            }

            else
            {
                //transform.DOMoveX(-1, 1).SetLoops(-1, LoopType.Yoyo);
                transform.DOMoveX(1, 1).SetLoops(-1, LoopType.Yoyo);
                //bulletPosition = transform.position;
                //transform.DOMoveX(1, 1);
                //transform.DOMove(bulletPosition, 1);
            }
        }
    }
}
