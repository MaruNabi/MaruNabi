using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public float moveSped = 10f;
    public float magnetDistance = 15f;
    
    private List<Transform> targets = new List<Transform>();
    
    private void Update()
    {
        if (targets.Count > 0)
        {
            if (targets.Count > 0)
            {
                foreach (var target in targets)
                {
                    if (Vector2.Distance(transform.position, target.position) <= magnetDistance)
                    {
                        Vector2 direction = (target.position - transform.position).normalized;
                        target.position = Vector2.MoveTowards(target.position, transform.position, moveSped * Time.deltaTime);
                    }
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!targets.Contains(other.transform))
            {
                targets.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (targets.Contains(other.transform))
            {
                targets.Remove(other.transform);
            }
        }
    }
}
