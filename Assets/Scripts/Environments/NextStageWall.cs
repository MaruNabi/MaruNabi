using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStageWall : MonoBehaviour
{
    [SerializeField] MouseManager mouseManager;
    public bool isClear;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isClear)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                mouseManager.NextStage();
            }
        }
    }
}
