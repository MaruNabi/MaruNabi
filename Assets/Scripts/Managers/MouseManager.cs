using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private bool isStart;
    [SerializeField] private float power = 10f;
    [SerializeField] private Rigidbody2D playerRigidbody;
    
    private void Update()
    {
        if (isStart)
        {
            playerRigidbody.AddForce(Vector3.left * power);
        }
    }
}