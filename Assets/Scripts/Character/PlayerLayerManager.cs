using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayerManager : MonoBehaviour
{
    private const int START_ORDER = 10;
    public Player playerMaru;
    public Player playerNabi;

    private Rigidbody2D mRigidBody;
    private Rigidbody2D nRigidBody;
    private SpriteRenderer mSpriteRenderer;
    private SpriteRenderer nSpriteRenderer;

    private List<SpriteRenderer> playerMoveList = new List<SpriteRenderer>();
    private List<SpriteRenderer> playerStopList = new List<SpriteRenderer>();
    
    void Start()
    {
        mRigidBody = playerMaru.GetComponent<Rigidbody2D>();
        nRigidBody = playerNabi.GetComponent<Rigidbody2D>();

        mSpriteRenderer = playerMaru.GetComponent<SpriteRenderer>();
        nSpriteRenderer = playerNabi.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (mRigidBody.velocity.normalized.x != 0)
        {
            if (!playerMoveList.Contains(mSpriteRenderer))
            {
                playerMoveList.Add(mSpriteRenderer);
            }
            if (playerStopList.Contains(mSpriteRenderer))
            {
                playerStopList.Remove(mSpriteRenderer);
            }
        }
        else
        {
            if (playerMoveList.Contains(mSpriteRenderer))
            {
                playerMoveList.Remove(mSpriteRenderer);
            }
            if (!playerStopList.Contains(mSpriteRenderer))
            {
                playerStopList.Add(mSpriteRenderer);
            }
        }

        if (nRigidBody.velocity.normalized.x != 0)
        {
            if (!playerMoveList.Contains(nSpriteRenderer))
            {
                playerMoveList.Add(nSpriteRenderer);
            }
            if (playerStopList.Contains(nSpriteRenderer))
            {
                playerStopList.Remove(nSpriteRenderer);
            }
        }
        else
        {
            if (playerMoveList.Contains(nSpriteRenderer))
            {
                playerMoveList.Remove(nSpriteRenderer);
            }
            if (!playerStopList.Contains(nSpriteRenderer))
            {
                playerStopList.Add(nSpriteRenderer);
            }
        }

        if (playerMoveList != null)
        {
            for (int i = 0; i < playerMoveList.Count; i++)
            {
                playerMoveList[i].sortingOrder = START_ORDER + 4 - i;
            }
        }
        else
            return;

        if (playerStopList != null)
        {
            for (int i = 0; i < playerStopList.Count; i++)
            {
                playerStopList[i].sortingOrder = START_ORDER + i;
            }
        }
        else
            return;
    }
}
