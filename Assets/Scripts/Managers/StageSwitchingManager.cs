using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class StageSwitchingManager : MonoBehaviour
{
    [Header("SKIP")]
    [SerializeField] private bool skipToStage2;
    [SerializeField] private bool skipToStage3;
    [SerializeField] private bool skipToStage4;
    [Space]

    [Header("CAMERAS")]
    [SerializeField] private CinemachineVirtualCamera stage2Camera;
    [SerializeField] private CinemachineVirtualCamera stage3Camera;
    [SerializeField] private CinemachineVirtualCamera stage4Camera;
    [SerializeField] private CinemachineVirtualCamera stage2ZoomCamera;
    [SerializeField] private CinemachineVirtualCamera stage3ZoomCamera;
    [Space]
    
    [Header("MANAGERS")]
    [SerializeField] private ScholarManager scholarManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private FoxManager foxManager;
    [SerializeField] private TigerManager tigerManager;
    [Space]

    [Header("SPAWN POINTS")]
    [SerializeField] private Transform stage2SpawnPoint;
    [SerializeField] private Transform stage3SpawnPoint;
    [SerializeField] private Transform stage4SpawnPoint;
    [Space]

    [SerializeField] private Transform targetGroup;
    
    private List<Player> players;
    private bool isStageClear;
    public int StageNumber { get; private set; }

    private void OnValidate()
    {
        if (skipToStage4)
        {
            skipToStage2 = false;
            skipToStage3 = false;
        }
        else if (skipToStage3)
        {
            skipToStage2 = false;
            skipToStage4 = false;
        }
        else if (skipToStage2)
        {
            skipToStage3 = false;
            skipToStage4 = false;
        }
    }

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<Player>()).ToList();
        StageNumber = 1;
        

        if (skipToStage4)
        {
            StageNumber = 4;
            scholarManager.gameObject.SetActive(false);
            mouseManager.gameObject.SetActive(false);
            foxManager.gameObject.SetActive(false);
            tigerManager.gameObject.SetActive(true);
            stage4Camera.gameObject.SetActive(true);
            players.ForEach(player => player.transform.position = stage4SpawnPoint.position);
            tigerManager.EnterProduction().Forget();
        }
        else if (skipToStage3)
        {
            StageNumber = 3;
            scholarManager.gameObject.SetActive(false);
            mouseManager.gameObject.SetActive(false);
            foxManager.gameObject.SetActive(true);
            foxManager.ProductionSkip().Forget();
            stage3Camera.gameObject.SetActive(true);
            players.ForEach(player => player.transform.position = stage3SpawnPoint.position + Vector3.right * 5f);
            
            Debug.Log("스킵");
        }
        else if (skipToStage2)
        {
            StageNumber = 2;
            scholarManager.gameObject.SetActive(false);
            players.ForEach(player => player.transform.position = stage2SpawnPoint.position);
            stage2Camera.gameObject.SetActive(true);
            mouseManager.StageStart();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            players.ForEach(player => player.ReviveCheat());
        }
    }

    private void FixedUpdate()
    {
        if (isStageClear)
        {
            // 우측으로 강제 이동
            players.ForEach(x => x.ForcedPlayerMoveToRight());
        }
    }

    public void ForcedMove()
    {
        // 행동 막기
        DisAllowBehavior();
        players.ForEach(x => x.IsTargetGround = false);

        stage2Camera.gameObject.SetActive(true);
        isStageClear = true;
    }

    public void AllowBehavior()
    {
        players.ForEach(x => x.PlayerStateTransition(true, 0));
    }

    public void DisAllowBehavior()
    {
        players.ForEach(x => x.PlayerStateTransition(false, 0));
    }

    public void StageStart(int _stageNumber)
    {
        switch (_stageNumber)
        {
            case 2:
                AllowBehavior();
                mouseManager.StageStart();
                StageNumber = 2;
                break;
            case 3:
                AllowBehavior();
                foxManager.StageStart().Forget();
                StageNumber = 3;
                break;
            default:
                break;
        }

    }

    public void ZoomIn(GameObject _target, int _stageNum)
    {
        if (_stageNum == 2)
        {
            stage2ZoomCamera.Follow = _target.transform;
            stage2ZoomCamera.Priority += 20;
            stage2ZoomCamera.gameObject.SetActive(true);
        }
        else if(_stageNum == 3)
        {
            stage3ZoomCamera.Follow = _target.transform;
            stage3ZoomCamera.Priority += 20;
            stage3ZoomCamera.gameObject.SetActive(true);
        }
    }

    public void ZoomOut()
    {
        stage2ZoomCamera.gameObject.SetActive(false);
        stage3ZoomCamera.gameObject.SetActive(false);
    }

    public void Stage3ClearProduction()
    {
        stage3Camera.gameObject.SetActive(true);
        stage3Camera.GetComponent<CinemachineVirtualCamera>().Follow = targetGroup;
        DOTween.To(() => 1, x=> stage3Camera.GetComponent<CinemachineStoryboard>().m_Alpha = x,0f, 2f)
            .OnStart(() =>
            {
                players.ForEach(player => player.transform.position = stage3SpawnPoint.position);
            })
            .OnComplete(() =>
            {
                
            });
    }

    public void Stage4Start()
    {
        players.ForEach(player => player.transform.DOMove(stage4SpawnPoint.position,1.8f));
        stage4Camera.gameObject.SetActive(true);
        stage3Camera.gameObject.SetActive(false);
        tigerManager.EnterProduction().Forget();
    }
}