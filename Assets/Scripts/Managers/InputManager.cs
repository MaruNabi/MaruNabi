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
            if (Input.GetAxis("Horizontal_J1") == 0.0f && Input.GetAxis("Horizontal_J2") == 0.0f)
            {
                if (isChangeOnce)
                {
                    isChangeOnce = false;
                    isNeedInit = true;
                }
                return;
            }
        }

        if (keyAction != null)
        {
            isChangeOnce = true;
            keyAction.Invoke();
        }
    }
}
