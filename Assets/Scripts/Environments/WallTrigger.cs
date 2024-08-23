using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [FormerlySerializedAs("switchingManager")] [SerializeField] StageManager manager;
    [SerializeField] private int stageNumber;
    private Collider2D collider2D;
    private int count;
    private GameObject[] players;
    
    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Utils.GetOrAddComponent<Player>(other.gameObject).IsTargetGround= true;
            count++;
            
            foreach (var player in players)
            {
                if (player.GetComponent<Player>().isPlayerDead)
                {
                    count++;
                }
            }
            
            if (count >= 2)
            {
                switch (stageNumber)
                {
                    case 1:
                        wall.SetActive(true);
                        collider2D.enabled = false;
                        manager.StageStart(2);
                        break;
                    case 2:
                        wall.SetActive(true);
                        collider2D.enabled = false;
                        manager.StageStart(3);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
