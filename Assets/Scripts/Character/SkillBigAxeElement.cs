using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigAxeElement : MonoBehaviour
{
    [SerializeField] private Transform axeRayStartPosition;
    [SerializeField] private LayerMask isLayer;

    private RaycastHit2D rayAxe;
    private float rayDistance = 0.9f;
    private bool isHitOnce = true;
    private string currentHit;
    private bool isEnemy = false;

    private float attackPower = 300.0f;

    void Update()
    {
        DrawRayAxe();
        BigAxeHit();
    }

    private void OnDisable()
    {
        isEnemy = false;
        currentHit = "";
    }

    private void DrawRayAxe()
    {
        rayAxe = Physics2D.Raycast(axeRayStartPosition.position, transform.right, rayDistance, isLayer);

        Debug.DrawRay(axeRayStartPosition.position, transform.right * rayDistance, Color.red);
    }

    private void BigAxeHit()
    {
        if (rayAxe.collider != null)
        {
            if (rayAxe.collider.tag == "Enemy" && isHitOnce)
            {
                isHitOnce = false;
                currentHit = rayAxe.collider.name;
                Debug.Log("Hit");
            }

            else if (rayAxe.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = rayAxe.collider.name;
                Debug.Log("Hit");
            }
        }

        else if (rayAxe.collider == null)
        {
            isHitOnce = true;
        }

        if (rayAxe.collider != null)
        {
            if (rayAxe.collider.tag == "Enemy" || rayAxe.collider.tag == "NoBumpEnemy")
                isEnemy = true;
            else
                isEnemy = false;

            if (isEnemy && isHitOnce)
            {
                isHitOnce = false;
                currentHit = rayAxe.collider.name;
                rayAxe.collider.GetComponent<Entity>().OnDamage(attackPower * 2);
            }

            else if (isEnemy && rayAxe.collider.name != currentHit)
            {
                isHitOnce = false;
                currentHit = rayAxe.collider.name;
                rayAxe.collider.GetComponent<Entity>().OnDamage(attackPower * 2);
            }
        }

        else if (rayAxe.collider == null)
        {
            isHitOnce = true;
        }
    }
}
