using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageWall : MonoBehaviour
{
    [SerializeField] StageSwitchingManager switchingManager;

    public bool isClear;
    
    private Collider2D wallCollider;
    
    private void Start()
    {
        wallCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isClear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                wallCollider.isTrigger = true;
                switchingManager.ForcedMove();
            }
        }
    }
}
