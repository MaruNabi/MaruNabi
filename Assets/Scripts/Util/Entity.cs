using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public abstract class Entity : MonoBehaviour
{
    public float HP { get; protected set; }
    public bool Dead { get; protected set; }
    public MonsterData Data { get; protected set; }

    protected float maxHP;
    
    private void Start()
    {
        StartCoroutine(WaitDataManager());
    }

    private IEnumerator WaitDataManager()
    {
        yield return new WaitUntil(() => Managers.Data != null);
        Init();
    }
    
    protected abstract void Init();

    protected virtual void OnEnable()
    {
        Dead = false;
        HP = maxHP;
    }

    public virtual void OnDamage(float _damage)
    {
        HP -= _damage;
        
        if (HP <= 0)
            HP = 0;
    }

    public virtual void RestoreHp(float _restoreHP)
    {
        if (Dead) return;

        if (maxHP >= HP + _restoreHP)
            HP += _restoreHP;
        else HP = maxHP;
    }

    public virtual void OnDead()
    {
        Dead = true;
        Destroy(gameObject);
    }
}
