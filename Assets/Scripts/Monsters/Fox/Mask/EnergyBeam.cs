using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBeam : MonoBehaviour
{
    private bool isJumpingBeam;

    public void Init(bool _isJumpingBeam)
    {
        isJumpingBeam = _isJumpingBeam;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Player>(out var player))
            {
                if(player.IsInvincibleTime)
                    return;
                
                if (isJumpingBeam)
                {
                    if (player.IsJumping)
                        player.PlayerHitSpecial(transform.position);
                }
                else
                {
                    player.PlayerHitSpecial(transform.position);
                }
            }
        }
    }
}