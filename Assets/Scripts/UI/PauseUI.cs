using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public static bool isGamePaused;
    public static bool isSetOnce = false;

    public GameObject pauseUI;

    [SerializeField] private Player playerMaru;
    [SerializeField] private Player playerNabi;

    void Start()
    {
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = true;
            isSetOnce = false;
        }

        if (!isSetOnce)
        {
            if (isGamePaused)
            {
                isSetOnce = true;
                pauseUI.SetActive(true);
                Time.timeScale = 0;
                playerMaru.PlayerInputDisable();
                playerNabi.PlayerInputDisable();
            }

            if (!isGamePaused)
            {
                isSetOnce = true;
                pauseUI.SetActive(false);
                Time.timeScale = 1f;
                playerMaru.PlayerInputEnable();
                playerNabi.PlayerInputEnable();
            }
        }
    }
}
