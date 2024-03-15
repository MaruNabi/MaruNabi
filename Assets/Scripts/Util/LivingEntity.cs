using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public int startingHP = 100;
    public int HP { get; protected set; }
    public bool dead { get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        HP = startingHP;
    }

    public virtual void OnDamage(int damage)
    {
        HP -= damage;
    }

    public virtual void RestoreHP(int restoreHP)
    {
        if (dead) return;

        if (startingHP >= HP + restoreHP)
            HP += restoreHP;
        else HP = startingHP;
    }

    public virtual void Dead()
    {
        if (onDeath != null) onDeath();
        dead = true;
        Destroy(gameObject);
    }
}
