using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialScene : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject dummy1;
    [SerializeField] private GameObject dummy2;

    private TutorialCamera tutorialCamera;

    private GameObject playerMaru;
    private GameObject playerNabi;
    private float averagePlayerTransform;
    private Vector3 cameraPosition = new Vector3(0, 0, -10);

    private bool isSubStage1Clear = false;
    private bool isSubStage2Start = false;
    private bool isSubStage2Clear = false;
    private bool isSubStageTransition = false;
    private float smoothCameraMoveSpeed = 5.0f;

    private string currentPlayer;

    void Start()
    {
        currentPlayer = "";

        tutorialCamera = mainCamera.GetComponent<TutorialCamera>();
        tutorialCamera.rightEndSize = 80.0f;

        dummy2.SetActive(false);

        playerMaru = GameObject.Find("Maru_Test");
        playerNabi = GameObject.Find("Nabi_Test");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
            SceneManager.LoadScene("StageSelectionScene");

        if (dummy1 == null && !isSubStage1Clear)
        {
            isSubStage1Clear = true;
            tutorialCamera.rightEndSize = 120.0f;
            isSubStageTransition = true;
        }

        if (isSubStage2Start)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition, Time.deltaTime * smoothCameraMoveSpeed);
            if (PlayerMaru.ultimateGauge != 1000.0f)
            {
                PlayerMaru.ultimateGauge = 1000.0f;
            }
        }

        if (dummy2 == null && !isSubStage2Clear)
        {
            isSubStage2Clear = true;
            isSubStage2Start = false;
            isSubStageTransition = true;
            tutorialCamera.rightEndSize = 160.0f;
        }

        if (isSubStageTransition)
        {
            SmoothCameraMove();
        }
    }

    private void SmoothCameraMove()
    {
        tutorialCamera.isSubStageMove = true;

        averagePlayerTransform = (playerMaru.transform.position.x + playerNabi.transform.position.x) / 2.0f;
        cameraPosition.x = averagePlayerTransform;
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition, Time.deltaTime * smoothCameraMoveSpeed);

        StartCoroutine(DelayTime());
    }

    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(1.0f);
        tutorialCamera.isSubStageMove = false;
        isSubStageTransition = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (currentPlayer == "")
                currentPlayer = collision.gameObject.name;
            else
            {
                if (collision.gameObject.name != currentPlayer)
                {
                    Debug.Log("if");
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    isSubStage2Start = true;
                    tutorialCamera.isSubStageMove = true;
                    cameraPosition.x = transform.position.x;
                    dummy2.SetActive(true);
                }
                else
                    currentPlayer = collision.gameObject.name;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            currentPlayer = "";
    }
}
