using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharm : Bullet
{
    private const float CHARM_ATTACK_POWER = 50.0f; 

    private List<GameObject> enemyList = new List<GameObject>();
    private Transform enemy;
    private float turnSpeed = 5.0f;
    private Vector3 bulletDirection;
    private Vector3 currentDirection;

    private bool isInitOnce = true;

    private float shortDis;
    private float currentZ;
    private float newZ;
    private float dot;
    private float angle;
    private float cross;

    void OnEnable()
    {
        if (PlayerNabi.isNabiTraitActivated)
            attackPower = CHARM_ATTACK_POWER * 1.5f;
        else
            attackPower = CHARM_ATTACK_POWER;

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

        Managers.Sound.PlaySFX("Charm");
        shootEffect = Resources.Load<GameObject>("Prefabs/VFX/Player/15Sprites/Dron");
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

        PlayShootEffect();

        AttackInstantiate();
    }

    private void OnDisable()
    {
        isInitOnce = true;
        enemy = null;
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        if (enemy != null)
        {
            bulletDirection = (enemy.position - transform.position).normalized;
            currentDirection = transform.right.normalized;

            dot = Vector3.Dot(currentDirection, bulletDirection);
            angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            cross = currentDirection.x * bulletDirection.y - currentDirection.y * bulletDirection.x;
            if (cross < 0)
                angle = -angle;

            bulletRigidbody.velocity = transform.right * speed;

            currentZ = transform.rotation.eulerAngles.z;
            newZ = Mathf.LerpAngle(currentZ, currentZ + angle, turnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, 0, newZ);
        }

        else
        {
            NormalBulletMovement();
        }

        ColliderCheck(true);
    }
}
