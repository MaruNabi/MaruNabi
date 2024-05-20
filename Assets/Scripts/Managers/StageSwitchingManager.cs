using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

public class StageSwitchingManager : MonoBehaviour
{
    [Header("SKIP")]
    [SerializeField] private bool skipToStage2;
    [Space]

    [SerializeField] private Transform stage2SpawnPosition;
    [SerializeField] private CinemachineVirtualCamera stage2Camera;
    [SerializeField] private CinemachineVirtualCamera zoomCamera;
    [SerializeField] private ScholarManager scholarManager;
    [SerializeField] private MouseManager mouseManager;

    private List<Player> players;

    private bool isStage1Clear;

    private void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player").Select(x => x.GetComponent<Player>()).ToList();

        if (skipToStage2)
        {
            scholarManager.Stage1Start = false;
            players.ForEach(player => player.transform.position = stage2SpawnPosition.position);
            stage2Camera.gameObject.SetActive(true);
            mouseManager.StageStart();
        }
    }

    private void FixedUpdate()
    {
        if (isStage1Clear)
        {
            // 우측으로 강제 이동
            players.ForEach(x => x.ForcedPlayerMoveToRight());
        }
    }

    public void ForcedMove()
    {
        // 행동 막기
        players.ForEach(x => x.PlayerStateTransition(false, 0));

        stage2Camera.gameObject.SetActive(true);
        isStage1Clear = true;
    }

    public void AllowBehavior()
    {
        players.ForEach(x => x.PlayerStateTransition(true, 0));
    }

    public void StageStart()
    {
        AllowBehavior();
        mouseManager.StageStart();
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
}