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
            {"Idle",new MouseScholarIdle(this)},
            {"Appearance",new MouseScholarScholarAppearance(this)},
            {"Fan",new MouseScholarFan(this)},
            {"Leave",new MouseScholarLeave(this)}
        };
    }

    public void Initialize(string _stateName, MouseScholar mouseScholar, Animator _mouseAnimator)
    {
        _mouseScholar = mouseScholar;
        mouseAnimator = _mouseAnimator;
        base.Initialize(_stateName);
    }

    public void ChangeAnimation(EScholarAnimationType _type)
    {
        switch (_type)
        {
            case EScholarAnimationType.Attack:
                mouseAnimator.SetTrigger("Attack");
                break;
            case EScholarAnimationType.Hit:
                mouseAnimator.SetTrigger("Hit");
                break;
            case EScholarAnimationType.Laugh:
                mouseAnimator.SetTrigger("Laugh");
                break;
            case EScholarAnimationType.Angry:
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