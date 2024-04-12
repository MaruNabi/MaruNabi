using System;
using Cinemachine;
using UnityEngine;

[Serializable]
public class BGScrollData
{
    public float leftPosX = 0f;
    public float rightPosX = 0f;
    public float xScreenHalfSize;
    public float yScreenHalfSize;
    
    public BGScrollData(float _xScreenHalfSize, float _yScreenHalfSize, int _length)
    {
        xScreenHalfSize = _xScreenHalfSize;
        yScreenHalfSize = _yScreenHalfSize;
        
        leftPosX = -(xScreenHalfSize * 2)/2;
        rightPosX = xScreenHalfSize * 2 * _length - (xScreenHalfSize+0.67f);
    }
}

public class ScrollManager : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] backgrounds;
    private BGScrollData scrollData;

    private void Start()
    {
        scrollData = new BGScrollData(Camera.main.orthographicSize * Camera.main.aspect, Camera.main.orthographicSize, backgrounds.Length);
        //backgrounds[0].position = new Vector3(scrollData.leftPosX, backgrounds[0].position.y, backgrounds[0].position.z);
    }

    private void Update()
    {
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