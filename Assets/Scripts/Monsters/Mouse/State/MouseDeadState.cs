using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class MouseDeadState : MouseState
{
    public MouseDeadState(MouseStateMachine mouseStateMachine) : base(mouseStateMachine) { }
}