using System;
using Cinemachine;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FrontTreeScroll : ScrollManager
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] backgrounds;
    private ScrollData scrollData;

    private void Start()
    {
        scrollData = new ScrollData(Camera.main.orthographicSize * Camera.main.aspect + 10f, Camera.main.orthographicSize, backgrounds.Length);
        //backgrounds[0].position = new Vector3(scrollData.leftPosX, backgrounds[0].position.y, backgrounds[0].position.z);
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
            backgrounds[i].position += new Vector3(-speed, 0, 0) * Time.deltaTime;
 
            if(backgrounds[i].position.x < scrollData.leftPosX)
            {
                Vector3 nextPos = backgrounds[i].position;
                nextPos = new Vector3(scrollData.rightPosX, nextPos.y, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }
}