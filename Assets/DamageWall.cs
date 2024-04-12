using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit Wall");
            collision.gameObject.transform.DOJump(collision.gameObject.transform.position+Vector3.right*2f, 1f, 1, 0.35f);
        }
    }
}