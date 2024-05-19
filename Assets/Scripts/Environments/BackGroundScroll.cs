using System;
using Cinemachine;
using UnityEngine;

public class BackGroundScroll : ScrollManager
{
    [SerializeField] private Transform[] backgrounds;
    private ScrollData scrollData;

    private void Start()
    {
        scrollData = new ScrollData(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize, backgrounds.Length);
    }

    private void Update()
    {
        if(isStart)
            UpdateScroll();
    }

    void UpdateScroll()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += new Vector3(-speed, -speed/8, 0) * Time.deltaTime;
 
            if(backgrounds[i].position.x < scrollData.leftPosX)
            {
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(scrollData.rightPosX, 20f, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }
}