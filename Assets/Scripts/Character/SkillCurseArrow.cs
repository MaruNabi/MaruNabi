using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillCurseArrow : Bullet
{
    private SpriteRenderer arrowSpriteRenderer;

    private Vector3 originPosition = new Vector3(5, 0, 0);

    private Color originColor = new Color(1, 1, 1, 1);
    private Color fadeColor = new Color(1, 1, 1, 0);

    private void OnEnable()
    {
        arrowSpriteRenderer = GetComponent<SpriteRenderer>();
        arrowSpriteRenderer.color = fadeColor;

        Managers.Sound.PlaySFX("CArrow");
        StartCoroutine(ArrowShoot());
    }

    void Update()
    {
        if (this.gameObject.activeSelf)
        {
            AttackInstantiate();
        }
    }

    private void OnDisable()
    {
        transform.localPosition = originPosition;
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        ColliderCheck(false);
    }

    private IEnumerator ArrowShoot()
    {
        arrowSpriteRenderer.DOFade(1, 0.2f);
        yield return new WaitForSeconds(0.2f);

        transform.DOLocalMove(new Vector3(0.6f, 0, 0), 0.3f).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(0.3f);

        Managers.Sound.PlaySFX("CArrowHit");
        //enemyName = ray.collider.name;
        //ray.collider.GetComponent<Entity>().OnDamage(attackPower * 2);
    }
}
