using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillBigAxe : Sword
{
    private const int objSize = 4;
    private float deg;
    private float objSpeed = -100.0f;
    private float radius = 3.0f;

    [SerializeField]
    private GameObject[] axe = new GameObject[objSize];

    private bool isReady;
    
    private void OnEnable()
    {
        StartCoroutine(InitAxe());

        SetSword(7.0f);

        isReady = false;   
    }

    void Update()
    {
        if (isReady)
        {
            Revolution();
        }
    }

    private void OnDisable()
    {
        isReady = false;
        deg = 0;
        for (int i = 0; i < axe.Length; i++)
        {
            axe[i].transform.position = swordReturnPosition.transform.position;
            axe[i].transform.rotation = Quaternion.Euler(0, 0, i * 90);
        }
    }

    private IEnumerator InitAxe()
    {
        yield return new WaitForSeconds(0.01f);

        for (int i = 0; i < axe.Length; i++)
        {
            axe[i].transform.position = swordReturnPosition.transform.position;
            var quaternion = axe[i].transform.rotation;
            Vector3 newDirection = axe[i].transform.position + quaternion * Vector3.right * radius;
            axe[i].transform.DOMove(newDirection, 0.25f);
        }

        yield return new WaitForSeconds(0.3f);
        isReady = true;
    }

    private void Revolution()
    {
        deg += Time.deltaTime * objSpeed;

        
        if (deg < 360)
        {
            for (int i = 0; i < axe.Length; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / objSize)));
                var x = radius * Mathf.Sin(rad);
                var y = radius * Mathf.Cos(rad);
                axe[i].transform.position = swordReturnPosition.transform.position + new Vector3(x, y);
                axe[i].transform.rotation = Quaternion.Euler(0, 0, (deg + (i * (360 / objSize))) * -5);
                //axe[i].transform.RotateAround(swordReturnPosition.transform.position, Vector3.back, objSpeed * Time.deltaTime);
            }
        }
        else
            deg = 0;
    }

}
