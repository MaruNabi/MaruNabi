using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageWall : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = Utils.GetOrAddComponent<Player>(collision.gameObject);

            if (player.IsInvincibleTime == false && player.GetIsDeadBool() == false)
            {
                player.PlayerHit(transform.position);
                collision.gameObject.transform.DOJump(collision.gameObject.transform.position+Vector3.right*2f, 1f, 1, 0.3f);
            }
        }
    }
}