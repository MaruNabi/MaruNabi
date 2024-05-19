using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class MouseManager : MonoBehaviour
{
    [SerializeField] private float phase2Speed;
    [SerializeField] private Mouse mouse;
    [SerializeField] private List<ScrollManager> backGroundScrolls;
    [SerializeField] private SurfaceEffector2D surfaceEffector2D;
    [SerializeField] private StageSwitchingManager stageSwitchingManager;
    
    private bool stage2Start;
    private bool productionEnd;

    public bool Stage2Start => stage2Start;

    private void Start()
    {
        Mouse.MovingBackGround += BackGroundMove;
        Mouse.Phase2 += EffectorSpeedUp;
        Mouse.StageClear += StageClear;
    }

    private void OnDestroy()
    {
        Mouse.MovingBackGround -= BackGroundMove;
        Mouse.Phase2 -= EffectorSpeedUp;
        Mouse.StageClear -= StageClear;
    }
    
    public void StageStart()
    {
        mouse.enabled = true;
    }

    public void StageClear(GameObject _target)
    {
        if (productionEnd == false)
        {
            var list = GameObject.FindGameObjectsWithTag("Player");
            foreach (var item in list)
            {
                item.GetComponent<Player>().PlayerStateTransition(false,0);
            }
            stageSwitchingManager.ZoomIn(_target);
            productionEnd = true;
        }
        else
        {
            ZoomOutDelay().Forget();
        }
    }

    private async UniTaskVoid ZoomOutDelay()
    {
        stageSwitchingManager.ZoomOut();
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        var list = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in list)
        {
            item.GetComponent<Player>().PlayerStateTransition(true,0);
        }
    }

    private void BackGroundMove(bool _set)
    {
        ScrollDelay(_set).Forget();
    }

    private async UniTaskVoid ScrollDelay(bool _set)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        backGroundScrolls.ForEach(scroll =>
        {
            scroll.SetIsStart(_set);
        });
        stage2Start = _set;
        surfaceEffector2D.enabled = _set;
    }

    private void EffectorSpeedUp()
    {
        surfaceEffector2D.speed -= phase2Speed;

        foreach (ScrollManager scroll in backGroundScrolls)
        {
            if (scroll is MaterialScroll)
            {
                if ((scroll as MaterialScroll).IsSkyMaterial)
                {
                    scroll.SetSpeed(0.001f);
                    continue;
                }
            }
                
            scroll.SetSpeed(0.1f);
        }
    }
}