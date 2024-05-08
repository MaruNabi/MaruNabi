using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDeleteWall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TempObstacle"))
        {
            Destroy(collision.gameObject);
        }
    }
}