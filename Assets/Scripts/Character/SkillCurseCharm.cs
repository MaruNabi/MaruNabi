using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCurseCharm : Bullet
{
    private const float SCHARM_ATTACK_POWER = 450.0f; //800

    private List<GameObject> enemyList = new List<GameObject>();
    private Transform enemy;
    private float turnSpeed = 5.0f;
    private Vector3 bulletDirection;

    private bool isInitOnce = true;
    protected bool isHitEnemy = false;

    private float shortDis;
    private float bulletAngle;
    private float currentZ;
    private float newZ;

    [SerializeField]
    private GameObject curseArrow;

    private Animator curseCharmAnimator;

    void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = SCHARM_ATTACK_POWER * 2.3f;
        else
            attackPower = SCHARM_ATTACK_POWER;

        SetBullet();

        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        enemyList.AddRange(GameObject.FindGameObjectsWithTag("NoBumpEnemy"));

        if (enemyList.Count != 0)
        {
            enemy = enemyList[0].transform;

            shortDis = Vector3.Distance(gameObject.transform.position, enemyList[0].transform.position);

            foreach (GameObject found in enemyList)
            {
                float distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

                if (distance < shortDis)
                {
                    enemy = found.transform;
                    shortDis = distance;
                }
            }
        }

        Managers.Sound.PlaySFX("CCharm");
        curseCharmAnimator = GetComponent<Animator>();
        curseArrow.SetActive(false);
    }

    void Update()
    {
        if (transform.rotation == Quaternion.Euler(0, 0, 0) && isInitOnce && enemy != null) //transform.rotation == Quaternion.Euler(0, 0, 0)
        {
            isInitOnce = false;
            transform.rotation = Quaternion.Euler(0, 0, -180);
        }
        else if (transform.rotation != Quaternion.Euler(0, 0, 0) && isInitOnce && enemy != null)
        {
            isInitOnce = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (!isHitEnemy)
        {
            AttackInstantiate();
        }
        else
        {
            bulletRigidbody.velocity = new Vector2(0, 0);
            //transform.rotation = Quaternion.Euler(0, 180, 0);

            if (bulletDirection.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            curseArrow.SetActive(true);
        }
    }

    private void OnDisable()
    {
        isInitOnce = true;
        enemy = null;
        isHitEnemy = false;
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        if (enemy != null)
        {
            bulletDirection = (enemy.position - transform.position).normalized;

            bulletRigidbody.velocity = transform.right * speed;

            bulletAngle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
            currentZ = transform.rotation.eulerAngles.z;
            newZ = Mathf.LerpAngle(currentZ, bulletAngle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newZ);
        }

        else
        {
            NormalBulletMovement();
        }

        CurseCharmHit();
    }

    private void CurseCharmHit()
    {
        if (ray.collider != null)
        {
            if (ray.collider.tag == "Enemy" || ray.collider.tag == "NoBumpEnemy")
            {
                curseCharmAnimator.SetBool("isHit", true);
                Managers.Sound.PlaySFX("CCharmAtt");
                isHitEnemy = true;
            }
        }
    }
}
