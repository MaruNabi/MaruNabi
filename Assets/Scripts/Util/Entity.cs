using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Entity : MonoBehaviour
{
    protected float maxHP;
    public float HP { get; protected set; }
    public bool dead { get; protected set; }
    public event Action onDeath;

    protected MonsterData data;
    private void Start()
    {
        StartCoroutine(WaitDataManager());
    }

    IEnumerator WaitDataManager()
    {
        yield return new WaitUntil(() => Managers.Data != null);
        Init();
    }
    
    protected abstract void Init();

    protected virtual void OnEnable()
    {
        dead = false;
        HP = maxHP;
    }

    public virtual void OnDamage(int damage)
    {
        HP -= damage;
    }

    public virtual void RestoreHP(int restoreHP)
    {
        if (dead) return;

        if (maxHP >= HP + restoreHP)
            HP += restoreHP;
        else HP = maxHP;
    }

    public virtual void Dead()
    {
        if (onDeath != null) onDeath();
        dead = true;
        Destroy(gameObject);
    }
}
