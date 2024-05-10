using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [SerializeField] StageSwitchingManager switchingManager;
    private Collider2D collider2D;
    private int count;

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Utils.GetOrAddComponent<Player>(other.gameObject).IsTargetGround= true;
            count++;
            
            if (count >= 2)
            {
                wall.SetActive(true);
                switchingManager.StageStart();
                collider2D.enabled = false;
            }
        }
    }
}
