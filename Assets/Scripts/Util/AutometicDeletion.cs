using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutometicDeletion : MonoBehaviour
{
    [SerializeField] private float time;
    
    private void Start()
    {
        Destroy(gameObject, time);
    }
}