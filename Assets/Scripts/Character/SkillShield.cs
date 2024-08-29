using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShield : MonoBehaviour
{
    public int SkillDefence { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            SkillDefence += 1;
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            SkillDefence += 1;
            Destroy(collision.gameObject);
        }
    }
}
