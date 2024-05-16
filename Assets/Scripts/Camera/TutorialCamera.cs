using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCamera : MonoBehaviour
{
    private Vector3 cameraPosition = new Vector3(0, 0, -10);

    private Transform playerMaruTransform;
    private Transform playerNabiTransform;

    [SerializeField] private Vector2 mapSize;
    [SerializeField] private Vector2 center;

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
        CameraMoveAndLimit();
    }

    private void CameraMoveAndLimit()
    {
        averagePlayerPosition = (playerMaruTransform.position.x + playerNabiTransform.position.x) / 2.0f;
        cameraPosition.x = averagePlayerPosition;
        transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * cameraMoveSpeed);

        float lx = mapSize.x - cameraWidth;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        transform.position = new Vector3(clampX, 0, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
