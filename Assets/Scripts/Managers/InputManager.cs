using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static bool isNeedInit = false;
    public Action keyAction = null;

    private bool isChangeOnce = true;

    public void OnUpdate()
    {
        if (Input.anyKey == false)
        {
            if (isChangeOnce)
            {
                isChangeOnce = false;
                isNeedInit = true;
            }
            return;
        }

        if (keyAction != null)
        {
            isChangeOnce = true;
            keyAction.Invoke();
        }
    }
}
