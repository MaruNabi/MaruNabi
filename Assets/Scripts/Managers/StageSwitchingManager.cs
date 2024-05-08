using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSwitchingManager : MonoBehaviour
{
    [SerializeField] Player player1;
    [SerializeField] Player player2;

    private bool isClear;
    
    private void FixedUpdate()
    {
        if (isClear)
        {
            player1.ForcedPlayerMoveToRight();
            player2.ForcedPlayerMoveToRight();
        }
    }
    
    public void ForcedMove()
    {
        player1.PlayerStateTransition(false, 0);
        player2.PlayerStateTransition(false, 0);

        isClear = true;
    }
    
    public void AllowBehavior()
    {
        player1.PlayerStateTransition(true, 0);
        player2.PlayerStateTransition(true, 0);
    }
}
