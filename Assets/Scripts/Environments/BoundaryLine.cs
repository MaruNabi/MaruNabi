using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class BoundaryLine : MonoBehaviour
{
    [SerializeField] private Transform resetPoint;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().PlayerHitSpecial(Vector3.zero);
            collision.gameObject.transform.position = resetPoint.position;
        }
    }
}
