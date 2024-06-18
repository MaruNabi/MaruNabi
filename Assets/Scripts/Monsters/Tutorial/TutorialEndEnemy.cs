using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialEndEnemy : MonoBehaviour
{
    private bool isMaruIn = false;
    private bool isNabiIn = false;

    [SerializeField] private Player playerMaru;
    [SerializeField] private Player playerNabi;

    void Update()
    {
        if (isMaruIn)
        {
            if (Input.anyKeyDown)
            {
                playerMaru.PlayerInputDisable();
                playerNabi.PlayerInputDisable();
                SceneManager.LoadScene("DialogueScene");
            }
        }

        if (isNabiIn)
        {
            if (Input.anyKeyDown)
            {
                playerMaru.PlayerInputDisable();
                playerNabi.PlayerInputDisable();
                SceneManager.LoadScene("DialogueScene");
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
