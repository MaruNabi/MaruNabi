using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static bool isGamePaused = false;

    public GameObject pauseUI;

    void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = true;
        }

        if (isGamePaused)
        {
            pauseUI.SetActive(true);
            Time.timeScale = 0;
        }

        if (!isGamePaused)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
