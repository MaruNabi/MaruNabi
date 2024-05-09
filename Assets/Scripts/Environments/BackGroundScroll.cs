using System;
using UnityEngine;

public class BackGroundScroll : ScrollManager
{
    [SerializeField] private float speed;
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
                nextPos = new Vector3(scrollData.rightPosX, scrollData.rightPosY, nextPos.z);
                backgrounds[i].position = nextPos;
            }
        }
    }
}