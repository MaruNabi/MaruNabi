using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TailAttack : MonoBehaviour
{
    private Sequence sequence;
    private Vector3 startPos;
    
    private void Start()
    {
        startPos = transform.position;
    }
}
