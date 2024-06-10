using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2UI : MonoBehaviour
{
    public static bool isGamePaused;
    public static bool isSetOnce;

    public GameObject pauseUI;
    public GameObject gameOverUI;
    public GameObject progressBarUI;

    [SerializeField] private Player playerMaru;
    [SerializeField] private Player playerNabi;

    [SerializeField] private Image[] phaseBar;
    [SerializeField] private GameObject[] playerProgress;

    //StageSwitchingManager stageSwitchingManager = new StageSwitchingManager();
    public GameObject s_Manager;
    public GameObject s2_Manager;
    private ScholarManager scholarManager;
    private StageSwitchingManager stageSwitchingManager;

    private int[,] playerProcess = new int[2, 2];

    private List<GameObject> enemyList = new List<GameObject>();
    private float currentEnemyHp;
    private float playerProcessPos;
    private bool canChange;

    void Start()
    {
        isGamePaused = false;
        pauseUI.SetActive(false);
        gameOverUI.SetActive(false);
        progressBarUI.SetActive(false);

        stageSwitchingManager = s_Manager.GetComponent<StageSwitchingManager>();
        scholarManager = s2_Manager.GetComponent<ScholarManager>();

        StartCoroutine("EnemySet");
        canChange = false;
    }

    void Update()
    {
        if (!isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isGamePaused = true;
                isSetOnce = false;
            }
        }

        if (!isSetOnce && canChange)
        {
            if (isGamePaused)
            {
                isSetOnce = true;
                pauseUI.SetActive(true);
                if (!PlayerMaru.isDead && !PlayerNabi.isDead)
                {
                    CurrentProgress(0);
                    CurrentProgress(1);
                }
                else if (PlayerMaru.isDead)
                {
                    DeadProgress(0);
                    CurrentProgress(1);
                }
                else
                {
                    CurrentProgress(0);
                    DeadProgress(1);
                }
                progressBarUI.SetActive(true);
                Time.timeScale = 0;
                playerMaru.PlayerInputDisable();
                playerNabi.PlayerInputDisable();
            }

            if (!isGamePaused)
            {
                isSetOnce = true;
                pauseUI.SetActive(false);
                progressBarUI.SetActive(false);
                Time.timeScale = 1f;
                playerMaru.PlayerInputEnable();
                playerNabi.PlayerInputEnable();
            }
        }

        if (PlayerMaru.isDead && playerProcess[0, 0] == 0)
            PlayerDeadSave(0);
        if (PlayerNabi.isDead && playerProcess[1, 0] == 0)
            PlayerDeadSave(1);

        if (PlayerMaru.isDead && PlayerNabi.isDead)
        {
            gameOverUI.SetActive(true);
            progressBarUI.SetActive(true);
            DeadProgress(0);
            DeadProgress(1);
            canChange = false;
            //Time.timeScale = 0;
        }
    }

    private void CurrentProgress(int _player)
    {
        switch (stageSwitchingManager.StageNumber)
        {
            case 1:
                currentEnemyHp = scholarManager.MouseHp;
                playerProcessPos = (phaseBar[0].transform.localPosition.x - 53f) + ((1 - currentEnemyHp / 6000.0f) * 106f);
                playerProgress[_player].transform.localPosition = new Vector3(playerProcessPos, playerProgress[_player].transform.localPosition.y, 0.0f);
                break;
            case 2:
                currentEnemyHp = enemyList[1].GetComponent<Entity>().HP;
                playerProcessPos = (phaseBar[1].transform.localPosition.x - 69f) + ((1 - currentEnemyHp / 6000.0f) * 138f);
                playerProgress[_player].transform.localPosition = new Vector3(playerProcessPos, playerProgress[_player].transform.localPosition.y, 0.0f);
                break;
            case 3:
                currentEnemyHp = enemyList[2].GetComponent<Entity>().HP;
                playerProcessPos = (phaseBar[2].transform.localPosition.x - 103.5f) + ((1 - currentEnemyHp / 6000.0f) * 207f);
                playerProgress[_player].transform.localPosition = new Vector3(playerProcessPos, playerProgress[_player].transform.localPosition.y, 0.0f);
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    private void PlayerDeadSave(int _player)
    {
        playerProcess[_player, 0] = stageSwitchingManager.StageNumber;
        if (stageSwitchingManager.StageNumber == 1)
            playerProcess[_player, 1] = (int)scholarManager.MouseHp;
        else
            playerProcess[_player, 1] = (int)enemyList[stageSwitchingManager.StageNumber - 1].GetComponent<Entity>().HP;
    }

    private void DeadProgress(int _player)
    {
        switch (playerProcess[_player, 0])
        {
            case 1:
                playerProcessPos = (phaseBar[0].transform.localPosition.x - 53f) + ((1 - playerProcess[_player, 1] / 6000.0f) * 106f);
                playerProgress[_player].transform.localPosition = new Vector3(playerProcessPos, playerProgress[_player].transform.localPosition.y, 0.0f);
                break;
            case 2:
                playerProcessPos = (phaseBar[1].transform.localPosition.x - 69f) + ((1 - playerProcess[_player, 1] / 6000.0f) * 138f);
                playerProgress[_player].transform.localPosition = new Vector3(playerProcessPos, playerProgress[_player].transform.localPosition.y, 0.0f);
                break;
            case 3:
                playerProcessPos = (phaseBar[2].transform.localPosition.x - 103.5f) + ((1 - playerProcess[_player, 1] / 6000.0f) * 207f);
                playerProgress[_player].transform.localPosition = new Vector3(playerProcessPos, playerProgress[_player].transform.localPosition.y, 0.0f);
                break;
            case 4:
                break;
            default:
                break;
        }
    }

    private IEnumerator EnemySet()
    {
        yield return new WaitForSeconds(1.0f);
        enemyList.Add(GameObject.Find("MouseScholar"));
        enemyList.Add(GameObject.Find("Mouse"));
        enemyList.Add(GameObject.Find("Fox"));
        canChange = true;
    }
}
