using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTestManager : MonoBehaviour
{
    [SerializeField] DialogSystem dialogSystem;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            dialogSystem.DialogNextButtonClicked();
        }
    }
}
