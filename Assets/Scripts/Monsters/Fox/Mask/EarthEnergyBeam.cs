using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEnergyBeam : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                if (player.IsInvincibleTime)
                    return;

                player.PlayerHitSpecial(transform.position);
            }
        }
    }
}