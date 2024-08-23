using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class NextStageWall : MonoBehaviour
{
    [FormerlySerializedAs("switchingManager")] [SerializeField] StageManager manager;
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
            manager.Stage4Start();
        }
        else if(isStage3Clear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                wallCollider.isTrigger = true;
                manager.ForcedMove();
                DOTween.To(() => 0, x=> storyboardCamera.m_Alpha = x,1f, 2f)
                    .OnComplete(() =>
                    {
                        manager.Stage3ClearProduction();
                    });
            }
        }
        else if (isStage2Clear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                wallCollider.isTrigger = true;
                manager.ForcedMove();
            }
        }
        else if (isStage1Clear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                wallCollider.isTrigger = true;
                manager.ForcedMove();
                
                var playersObjects = GameObject.FindGameObjectsWithTag("Player");
                var players = playersObjects.Select(player => player.GetComponent<Player>()).ToList();
                foreach (var player in players)
                {
                    if (player != collision.gameObject.GetComponent<Player>())
                    {
                        player.transform.position = collision.gameObject.transform.position + Vector3.left * 2f;
                    }
                }
                
            }
        }
    }
}
