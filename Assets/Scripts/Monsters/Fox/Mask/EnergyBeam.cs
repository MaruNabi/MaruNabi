using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBeam : MonoBehaviour
{
    private bool isJumpingBeam;
    private float endTime;
    private bool isHit;

    public void Init(bool _isJumpingBeam)
    {
        isJumpingBeam = _isJumpingBeam;
        endTime = 0;
    }

    private void Update()
    {
        endTime += Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent<Player>(out var player);

            if (endTime >= 0.25f && !isHit)
            {
                if (isJumpingBeam)
                {
                    if (player.IsJumping)
                        player.PlayerHitSpecial(transform.position);
                }
                else
                {
                    player.PlayerHitSpecial(transform.position);
                }
                isHit = true;
            }
        }
    }
}