using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEndEnemy : MonoBehaviour
{
    private bool isMaruIn = false;
    private bool isNabiIn = false;

    void Update()
    {
        if (isMaruIn)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("NextScene");
            }
        }

        if (isNabiIn)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("NextScene");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.name == "Maru_Test")
                isMaruIn = true;
            else if(collision.gameObject.name == "Nabi_Test")
                isNabiIn = true;
            else
            {
                isMaruIn = false;
                isNabiIn = false;
            }
        }
    }
}
