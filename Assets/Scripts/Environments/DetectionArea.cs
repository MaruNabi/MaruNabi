using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    public List<GameObject> Players { get; private set; }

    private void Start()
    {
        Players = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Players.Contains(collision.gameObject))
                return;
            
            Players.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Players.Contains(collision.gameObject))
                Players.Remove(collision.gameObject);
        }
    }
}