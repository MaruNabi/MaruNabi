using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = System.Numerics.Vector2;

public class BoundaryLine : MonoBehaviour
{
    [SerializeField] private Transform resetPoint;
    [SerializeField] private Transform reservePoint;
    [SerializeField] private GameObject cloud;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().PlayerHitSpecial(Vector3.zero);
            if (cloud.activeSelf)
                collision.gameObject.transform.position = resetPoint.position;
            else
                collision.gameObject.transform.position = reservePoint.position;
        }
    }
}
