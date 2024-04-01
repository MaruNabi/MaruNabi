using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBigAxe : Sword
{
    private int objSize = 4;
    private float circleR = 4.0f;
    private float deg;
    private float objSpeed = 3.0f;

    public float radius = 2.0f; // 반지름
    public float speed = 2.0f;  // 속도

    private float angle = 0;

    private void OnEnable()
    {
        SetSword();
    }

    void Update()
    {
        /*deg += Time.deltaTime * objSpeed;
        if (deg < 360)
        {
            for (int i = 0; i < objSize; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / objSize)));
                var x = circleR * Mathf.Sin(rad);
                var y = circleR * Mathf.Cos(rad);
                transform.position = transform.position + new Vector3(x, y);
                transform.rotation = Quaternion.Euler(0, 0, (deg + (i * (360 / objSize))) * -1);
            }
        }
        else
            deg = 0;*/
        angle += speed * Time.deltaTime;
        transform.position = swordReturnPosition.transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
    }


}
