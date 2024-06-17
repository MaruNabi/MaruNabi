using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShield : MonoBehaviour
{
    public int SkillDefence { get; private set; }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hi");
            SkillDefence += 1;
        }

        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("Hello");
            SkillDefence += 1;
            Destroy(collision.gameObject);
        }
    }
}
