using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private bool roundStart;
    [SerializeField] private float power = 10f;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Rigidbody2D playerRigidbody2;
    [SerializeField] private GameObject mouse;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject player2;
    [SerializeField] private List<ScrollManager> backGroundScrolls;
    [SerializeField] private GameObject stage3StartPosition;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera2;

    private void Update()
    {
        if(Input.GetKey(KeyCode.F2))
        {
            virtualCamera2.gameObject.SetActive(true);
            roundStart = false;
            backGroundScrolls.ForEach(scroll =>
            {
                scroll.SetIsStart(false);
            });
            player.transform.position = stage3StartPosition.transform.position;
            player2.transform.position = stage3StartPosition.transform.position;
        }
        
        if (roundStart)
        {
            playerRigidbody.AddForce(Vector3.left * power);
            playerRigidbody2.AddForce(Vector3.left * power);
        }
    }

    public void StageReady()
    {
        backGroundScrolls.ForEach(scroll =>
        {
            scroll.gameObject.SetActive(true);
        });
        virtualCamera.gameObject.SetActive(true);

        playerRigidbody=player.GetComponent<Rigidbody2D>();
        playerRigidbody2=player2.GetComponent<Rigidbody2D>();
    }

    public void StageStart()
    {
        backGroundScrolls.ForEach(scroll =>
        {
            scroll.SetIsStart(true);
        });
        mouse.GetComponent<Mouse>().enabled = true;
        roundStart = true;
    }
}