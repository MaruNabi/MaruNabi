using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private float power = 15f;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Rigidbody2D playerRigidbody2;
    [SerializeField] private Mouse mouse;
    [SerializeField] private List<ScrollManager> backGroundScrolls;
    [SerializeField] private SurfaceEffector2D surfaceEffector2D;
    
    private bool stage2Start;

    public bool Stage2Start => stage2Start;

    private void Start()
    {
        Mouse.MovingBackGround += BackGroundMove;
    }

    private void OnDestroy()
    {
        Mouse.MovingBackGround -= BackGroundMove;
    }

    private void Update()
    {
        if (stage2Start)
        {
            playerRigidbody.AddForce(Vector3.left * power);
            playerRigidbody2.AddForce(Vector3.left * power);
        }
    }

    public void StageStart()
    {
        mouse.enabled = true;
    }

    private void BackGroundMove(bool _set)
    {
        ScrollDelay(_set).Forget();
    }

    async UniTaskVoid ScrollDelay(bool _set)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        backGroundScrolls.ForEach(scroll =>
        {
            scroll.SetIsStart(_set);
        });
        stage2Start = _set;
        surfaceEffector2D.enabled = _set;
    }
}