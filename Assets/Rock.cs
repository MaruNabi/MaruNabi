using System;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using DG.Tweening;

public class Rock : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.left * 5f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Hit Rock");
            collision.gameObject.transform.DOJump(collision.gameObject.transform.position+Vector3.left*2f, 1f, 1, 0.35f);
        }
    }
}