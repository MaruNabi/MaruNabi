using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class NextStageWall : MonoBehaviour
{
    [SerializeField] StageSwitchingManager switchingManager;
    [SerializeField] CinemachineStoryboard storyboardCamera;

    public bool isStage1Clear;
    public bool isStage2Clear;
    public bool isStage3Clear;
    public bool isStage4Clear;

    
    private Collider2D wallCollider;
    
    private void Start()
    {
        wallCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStage4Clear)
        {
            wallCollider.isTrigger = true;
            switchingManager.Stage4Start();
        }
        else if(isStage3Clear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                wallCollider.isTrigger = true;
                switchingManager.ForcedMove();
                DOTween.To(() => 0, x=> storyboardCamera.m_Alpha = x,1f, 2f)
                    .OnComplete(() =>
                    {
                        switchingManager.Stage3ClearProduction();
                    });
            }
        }
        else if (isStage2Clear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                wallCollider.isTrigger = true;
                switchingManager.ForcedMove();
            }
        }
        else if (isStage1Clear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                wallCollider.isTrigger = true;
                switchingManager.ForcedMove();
            }
        }
    }
}
