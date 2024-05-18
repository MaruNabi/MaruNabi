using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    [SerializeField] private float leftEndSize = 11;
    public float rightEndSize;

    public bool isSubStageMove = false;

    private Vector3 cameraPosition = new Vector3(0, 0, -10);

    private Transform playerMaruTransform;
    private Transform playerNabiTransform;

    private float averagePlayerPosition;
    private float cameraMoveSpeed = 50.0f;
    private float cameraHeight;
    private float cameraWidth;

    void Start()
    {
        playerMaruTransform = GameObject.Find("Maru_Test").GetComponent<Transform>();
        playerNabiTransform = GameObject.Find("Nabi_Test").GetComponent<Transform>();

        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        if (!isSubStageMove)
        {
            CameraMoveAndLimit();
        }
    }

    private void CameraMoveAndLimit()
    {
        averagePlayerPosition = (playerMaruTransform.position.x + playerNabiTransform.position.x) / 2.0f;
        cameraPosition.x = averagePlayerPosition;
        transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * cameraMoveSpeed);
        
        float leftM = -leftEndSize + cameraWidth;
        float rightM = rightEndSize - cameraWidth;

        float clampX = Mathf.Clamp(transform.position.x, leftM, rightM);

        transform.position = new Vector3(clampX, 0, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(leftEndSize - cameraWidth, 0), new Vector2(rightEndSize - cameraWidth, 0));
    }
}
