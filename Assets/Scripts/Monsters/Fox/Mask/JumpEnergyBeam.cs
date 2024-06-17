using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEnergyBeam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<Player>().IsJumping)
            {
                other.GetComponent<Player>().PlayerHitSpecial(transform.position);
            }
        }
    }
}
