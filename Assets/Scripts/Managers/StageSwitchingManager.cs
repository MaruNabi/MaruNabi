using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class StageSwitchingManager : MonoBehaviour
{
    [Header("Stage2로 건너뛰기")]
    [SerializeField] private bool stage2Start;
    [Space]
    
    [SerializeField] private Player player2;
    [SerializeField] private Player player1;
    [SerializeField] private Transform stage2SpawnPosition;
    [SerializeField] private CinemachineVirtualCamera stage2Camera;
    [SerializeField] private CinemachineVirtualCamera zoomCamera;
    [SerializeField] private ScholarManager scholarManager;
    [SerializeField] private MouseManager mouseManager;
    
    private bool isStage1Clear;
    
    private void Start()
    {
        if (stage2Start)
        {
            scholarManager.Stage1Start = false;
            player1.transform.position = stage2SpawnPosition.position;
            player2.transform.position = stage2SpawnPosition.position;
            stage2Camera.gameObject.SetActive(true);
            mouseManager.StageStart();
        }
    }
    
    private void FixedUpdate()
    {
        if (isStage1Clear)
        {
            // 우측으로 강제 이동
            player1.ForcedPlayerMoveToRight();
            player2.ForcedPlayerMoveToRight();
        }
    }
    
    public void ForcedMove()
    {
        // 행동 막기
        player1.PlayerStateTransition(false, 0);
        player2.PlayerStateTransition(false, 0);

        stage2Camera.gameObject.SetActive(true);
        isStage1Clear = true;
    }
    
    public void AllowBehavior()
    {
        player1.PlayerStateTransition(true, 0);
        player2.PlayerStateTransition(true, 0);
    }
    
    public void StageStart()
    {
        AllowBehavior();
        mouseManager.StageStart();
    }
    
    public void ZoomIn(GameObject _target)
    {
        zoomCamera.Follow = _target.transform;
        zoomCamera.Priority = 20;
        zoomCamera.gameObject.SetActive(true);
    }
    
    public void ZoomOut()
    {
        zoomCamera.gameObject.SetActive(false);
        
    }
}