using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigAxe : Sword
{
    private const int objSize = 4;
    private float circleR = 10.0f;
    private float deg = 0.0f;
    private float objSpeed = 10.0f;

    [SerializeField]
    private GameObject[] axe = new GameObject[objSize];
    
    private void OnEnable()
    {
        SetSword();
    }

    void Update()
    {
        deg += Time.deltaTime * objSpeed;
        if (deg < 360)
        {
            for (int i = 0; i < objSize; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / objSize)));
                var x = circleR * Mathf.Sin(rad);
                var y = circleR * Mathf.Cos(rad);
                axe[i].transform.position = transform.position + new Vector3(x, y);
                axe[i].transform.rotation = Quaternion.Euler(0, 0, (deg + (i * (360 / objSize))) * -1);
            }
        }
        else
            deg = 0;
    }


}
