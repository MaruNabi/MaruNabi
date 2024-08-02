using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Random = System.Random;

public class FoxManager : MonoBehaviour
{
    [SerializeField] private StageSwitchingManager stageSwitchingManager;
    [SerializeField] private SoulProduction soul;
    [SerializeField] private NextStageWall nextStageWall;
    [SerializeField] private Fox fox;
    [SerializeField] private GameObject wallTrigger;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject clouds;
    
    private bool productionEnd;

    private void Start()
    {
        Fox.Stage3Clear += StageClear;
    }
    
    private void OnDestroy()
    {
        Fox.Stage3Clear -= StageClear;
    }
    
    public async UniTaskVoid StageStart()
    {
        stageSwitchingManager.DisableBehavior();
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        soul.gameObject.SetActive(true);
        soul.StartProduction();
        await UniTask.Delay(TimeSpan.FromSeconds(10f));
        fox.ChangeAnimation(EFoxAnimationType.Laugh);
        Managers.Sound.StopBGM();
        Managers.Sound.SetBGMVolume(1f);
        Managers.Sound.PlayBGM("Fox_Stage");
        fox.UseTailPhase1();
        stageSwitchingManager.EnableBehavior();
        // TODO: 여우 페이즈1 시작 들어가야 함
    }

    public void StageClear(GameObject _target)
    {
        if (productionEnd == false)
        {
            fox.transform.DOShakePosition(1f);

            var list = GameObject.FindGameObjectsWithTag("Player");
            foreach (var item in list)
            {
                item.GetComponent<Player>().PlayerInputDisable();
            }
            stageSwitchingManager.ZoomIn(_target,3);
            productionEnd = true;
        }
        else
        {
            ZoomOutDelay().Forget();
        }
        //soul.ClearProduction();
    }
    
    private async UniTaskVoid ZoomOutDelay()
    {
        stageSwitchingManager.ZoomOut();
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        var list = GameObject.FindGameObjectsWithTag("Player");
        foreach (var item in list)
        {
            item.GetComponent<Player>().PlayerInputEnable();
        }
        
        clouds.SetActive(true);
        clouds.transform.DOMoveY(-47, 2.5f);
        nextStageWall.GetComponent<NextStageWall>().isStage4Clear = true;
        Managers.Sound.StopBGM();
        stageSwitchingManager.HealPlayers();
    }

    public async UniTaskVoid ProductionSkip()
    {
        leftWall.SetActive(true);
        stageSwitchingManager.DisableBehavior();
        wallTrigger.gameObject.SetActive(false);
        soul.ProductionSkip();
        fox.ChangeAnimation(EFoxAnimationType.Laugh);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        stageSwitchingManager.EnableBehavior();
        fox.UseTailPhase1();
        Managers.Sound.StopBGM();
        Managers.Sound.SetBGMVolume(1f);
        Managers.Sound.PlayBGM("Fox_Stage");
        // TODO: 여우 페이즈1 시작 들어가야 함
    }
    
    public async UniTaskVoid StageSkip()
    {
        leftWall.SetActive(true);
        stageSwitchingManager.DisableBehavior();
        wallTrigger.gameObject.SetActive(false);
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        fox.OnDead();
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        clouds.SetActive(true);
        clouds.transform.DOMoveY(-47, 2.5f);
        nextStageWall.GetComponent<NextStageWall>().isStage4Clear = true;
    }
}