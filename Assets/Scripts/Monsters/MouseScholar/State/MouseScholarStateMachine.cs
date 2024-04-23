using System.Collections.Generic;
using UnityEngine;

public class MouseScholarStateMachine : StateMachine<MouseScholarStateMachine>
{
    public MouseScholar MouseScholar => _mouseScholar;
    private MouseScholar _mouseScholar;
    private Animator mouseAnimator;

    private void Awake()
    {
        base.states = new Dictionary<string, State<MouseScholarStateMachine>>{
            {"Idle",new MouseScholarScholarIdle(this)},
            {"Appearance",new MouseScholarScholarAppearance(this)},
            {"Fan",new MouseScholarScholarFan(this)},
            {"Leave",new MouseScholarScholarLeave(this)}
        };
    }

    public void Initialize(string _stateName, MouseScholar mouseScholar, Animator _mouseAnimator)
    {
        _mouseScholar = mouseScholar;
        mouseAnimator = _mouseAnimator;
        base.Initialize(_stateName);
    }

    public void ChangeAnimation(EAnimationType _type)
    {
        switch (_type)
        {
            case EAnimationType.Attack:
                mouseAnimator.SetTrigger("Attack");
                break;
            case EAnimationType.Hit:
                mouseAnimator.SetTrigger("Hit");
                break;
            case EAnimationType.Laugh:
                mouseAnimator.SetTrigger("Laugh");
                break;
            case EAnimationType.Angry:
                mouseAnimator.SetTrigger("Angry");
                break;
            default:
                break;
        }
    }

    public float GetAnimPlayTime()
    {
        return mouseAnimator.GetCurrentAnimatorStateInfo(0).length;
    }
}