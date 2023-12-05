using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCharm : Bullet
{
    private Transform enemy;
    float turn = 5.0f;

    void Start()
    {
        SetBullet();

        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        Debug.Log(transform.rotation);
    }

    void Update()
    {
        AttackInstantiate();
    }

    protected override void AttackInstantiate()
    {
        base.AttackInstantiate();

        //enemy = GameObject.FindGameObjectWithTag("Enemy").transform;

        /*Vector3 dir = transform.right;
        Vector3 direction = (enemy.position - this.transform.position).normalized;

        Vector3 crossVec = Vector3.Cross(dir, direction);

        float inner = Vector3.Dot(Vector3.forward, crossVec);

        float addAngle = inner > 0 ? speed * Time.fixedDeltaTime : -speed * Time.fixedDeltaTime;
        float saveAngle = addAngle + transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(0, 0, saveAngle);

        float moveDirAngle = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 moveDir = new Vector2(Mathf.Cos(moveDirAngle), Mathf.Sin(moveDirAngle));

        bulletRigidbody.velocity = moveDir;*/

        Vector3 bulletDirection = (enemy.position - transform.position).normalized;
        //bulletRigidbody.AddForce(bulletDirection, ForceMode2D.Impulse);

        bulletRigidbody.velocity = transform.right * speed;

        float angle = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
        float currentZ = transform.rotation.eulerAngles.z;
        float newZ = Mathf.LerpAngle(currentZ, angle, turn * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, 0, newZ);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, bulletTargetRotation, turn);

        /*float vx = direction.x * speed;
        float vy = direction.y * speed;

        bulletRigidbody.velocity = new Vector2(vx, vy);*/

        //transform.position += direction * speed * Time.deltaTime;
    } 
}
