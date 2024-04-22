using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private bool isStart;
    [SerializeField] private float power = 10f;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private GameObject mouse;
    [SerializeField] private GameObject player;
    [SerializeField] private List<ScrollManager> backGroundScrolls;
    [SerializeField] private GameObject stage2StartPosition;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        // if (isStart)
        // {
        //     mouse.GetComponent<Mouse>().enabled = true;
        //     backGroundScrolls.ForEach(scroll => scroll.SetIsStart(true));
        // }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.F1))
        {
            virtualCamera.gameObject.SetActive(true);
            player.transform.position = stage2StartPosition.transform.position;
            isStart = true;
            mouse.GetComponent<Mouse>().enabled = true;
            backGroundScrolls.ForEach(scroll => scroll.SetIsStart(true));
        }
        
        if (isStart)
        {
            playerRigidbody.AddForce(Vector3.left * power);
        }
    }
}