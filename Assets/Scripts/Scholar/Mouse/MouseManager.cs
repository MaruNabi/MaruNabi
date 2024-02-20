using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private bool isMouseBehit;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetSchloarBehit(bool isHit)
    {
        this.isMouseBehit = isHit;
    }

    public bool GetIsSchloarBehit()
    {
        return this.isMouseBehit;
    }
}
