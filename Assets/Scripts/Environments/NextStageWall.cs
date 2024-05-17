using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NextStageWall : MonoBehaviour
{
    [SerializeField] StageSwitchingManager switchingManager;

    public bool isStage1Clear;
    public bool isStage2Clear;
    
    private Collider2D wallCollider;
    
    private void Start()
    {
        wallCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStage2Clear)
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
