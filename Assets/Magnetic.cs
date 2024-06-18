using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{

    private bool magnetInZone;


    public Transform target;

    public float moveSpeed = 1.0f;

    private void Update()
    {
        if (magnetInZone)
        {
            Vector2 relativePos = target.transform.position - transform.position;
            target.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            magnetInZone = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            magnetInZone = false;
        }
    }
}
