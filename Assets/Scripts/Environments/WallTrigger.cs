using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [SerializeField] MouseManager mouseManager;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            wall.SetActive(true);
            Utils.GetOrAddComponent<Player>(other.gameObject).IsTargetGround= true;
            mouseManager.StageReady();
        }
    }
}
