using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startingHP = 100f;
    public float HP { get; protected set; }
    public bool dead { get; protected set; }

    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        HP = startingHP;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        HP -= damage;
        if (HP <= 0 && !dead)
        {
            Dead();
        }
    }

    public virtual void RestoreHP(float restoreHP)
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
    }
}
