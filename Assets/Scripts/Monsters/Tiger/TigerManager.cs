using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TigerManager : MonoBehaviour
{
    [SerializeField] private StageSwitchingManager stageSwitchingManager;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Tiger tiger;
    private CinemachineBasicMultiChannelPerlin cameraShake;

    private void Start()
    {
        cameraShake = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public async UniTaskVoid EnterProduction()
    {
        if (cameraShake == null)
            cameraShake = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        stageSwitchingManager.DisAllowBehavior();
        await UniTask.Delay(TimeSpan.FromSeconds(3f));
        Managers.Sound.PlaySFX("Tiger_Ground");
        cameraShake.m_FrequencyGain = 0.5f;
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        tiger.StageStartProduction();
        await UniTask.Delay(TimeSpan.FromSeconds(6.5f));
        cameraShake.m_AmplitudeGain = 3f;
        await UniTask.Delay(TimeSpan.FromSeconds(1.5f));
        cameraShake.m_AmplitudeGain = 0f;
        cameraShake.m_FrequencyGain = 0f;
        stageSwitchingManager.AllowBehavior();
    }
}