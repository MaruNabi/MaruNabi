using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarStateMachine : StateMachine<ScholarStateMachine>
{
    public Scholar Scholar => scholar;
    private Scholar scholar;
    private Animator scholarAnimator;

    private void Awake()
    {
        base.states = new Dictionary<string, State<ScholarStateMachine>>{
            {"Idle",new ScholarIdle(this)},
            {"Appearance",new ScholarAppearance(this)},
            {"Fan",new ScholarFan(this)},
            {"Leave",new ScholarLeave(this)},
            {"Punish", new ScholarPunish(this)}
        };
    }

    public void Initialize(string _stateName, Scholar _scholar, Animator _scholarAnimator)
    {
        scholar = _scholar;
        scholarAnimator = _scholarAnimator;
        base.Initialize(_stateName);
    }
    
    public void ChangeAnimation(EAnimationType _type)
    {
        switch (_type)
        {
            case EAnimationType.Attack:
                scholarAnimator?.SetTrigger("Attack");
                break;
            case EAnimationType.Hit:
                scholarAnimator?.SetTrigger("Hit");
                break;
            case EAnimationType.Laugh:
                scholarAnimator?.SetTrigger("Laugh");
                break;
            case EAnimationType.Angry:
                scholarAnimator?.SetTrigger("Angry");
                break;
            default:
                break;
        }
    }
    
    public float GetAnimPlayTime()
    {
        return scholarAnimator.GetCurrentAnimatorStateInfo(0).length;
    }
}
