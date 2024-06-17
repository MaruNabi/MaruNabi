using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CloudPlatform : MonoBehaviour
{
    public float bounceAmount = 0.2f; // �������� ����
    public float bounceSpeed = 5f; // �������� �ӵ�

    private Vector3 originalPosition;
    private bool isPlayerOnPlatform = false;
    private Transform player;
    private int layerMask;

    void Start()
    {
        originalPosition = transform.localPosition;
        layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        if (isPlayerOnPlatform)
        {
            // �÷����� �������� �κ�
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition - new Vector3(0, bounceAmount, 0), Time.deltaTime * bounceSpeed);
        }
        else
        {
            // �÷����� ���� ��ġ�� ���ƿ��� �κ�
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * bounceSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
            var groundRay = Physics2D.Raycast(player.position, Vector2.down,1f,layerMask);
            
            if(groundRay.collider != null)
            {
                isPlayerOnPlatform = true;
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            player = null;
        }
    }
}
