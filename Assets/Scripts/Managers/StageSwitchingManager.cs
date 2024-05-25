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
    [Space]

    [SerializeField] private CinemachineVirtualCamera stage2Camera;
    [SerializeField] private CinemachineVirtualCamera stage3Camera;
    [SerializeField] private CinemachineVirtualCamera zoomCamera;
    
    [SerializeField] private ScholarManager scholarManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private FoxManager foxManager;
    [SerializeField] private Transform stage2SpawnPoint;
    [SerializeField] private Transform stage3SpawnPoint;
    [SerializeField] private Transform targetGroup;
    private List<Player> players;

    private bool isStageClear;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<Player>()).ToList();

        if (skipToStage3)
        {
            scholarManager.StageStart = false;
            players.ForEach(player => player.transform.position = stage2SpawnPoint.position);
            stage2Camera.gameObject.SetActive(true);
            mouseManager.ProductionSkip();
            Debug.Log("스킵");
        }
        else if (skipToStage2)
        {
            scholarManager.StageStart = false;
            players.ForEach(player => player.transform.position = stage2SpawnPoint.position);
            stage2Camera.gameObject.SetActive(true);
            mouseManager.StageStart();
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
            case 1:
                AllowBehavior();
                mouseManager.StageStart();
                break;
            case 2:
                AllowBehavior();
                foxManager.StageStart().Forget();
                break;
            default:
                break;
        }

    }

    public void ZoomIn(GameObject _target)
    {
        zoomCamera.Follow = _target.transform;
        zoomCamera.Priority += 20;
        zoomCamera.gameObject.SetActive(true);
    }

    public void ZoomOut()
    {
        zoomCamera.gameObject.SetActive(false);
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
}