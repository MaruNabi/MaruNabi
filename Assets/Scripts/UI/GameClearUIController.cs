using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class GameClearUIController : MonoBehaviour
{
    private const float totalTagPosY = 170.0f;
    private const float pannelPosY = 0.0f;
    private const float playerTagPosY = 342.0f;

    public StageClearTimer Timer;
    public SkillShield maruShield;

    private int[] classJudgement = { 134780, 204978, 247097, 261137 };

    private int[] maruScore = new int[5]; //combet, trait, life, total, rank
    private int[] nabiScore = new int[5];

    private int clearTime;
    private int timeScore;
    private bool canNextStage;

    private KeyCode selectKey;

    [SerializeField] private PlayerMaru playerMaru;
    [SerializeField] private PlayerNabi playerNabi;

    [SerializeField] private Image backGround;
    [SerializeField] private Image totalTag;
    [SerializeField] private Image maruPannel;
    [SerializeField] private Image nabiPannel;
    [SerializeField] private Image maruTag;
    [SerializeField] private Image nabiTag;
    [SerializeField] private TMP_Text[] clearTimeText;
    [SerializeField] private TMP_Text[] maruText;
    [SerializeField] private TMP_Text[] nabiText;
    [SerializeField] private Image[] maruLife;
    [SerializeField] private Image[] nabiLife;
    [SerializeField] private Sprite[] lifeSprite; 
    [SerializeField] private Image maruStamp;
    [SerializeField] private Image nabiStamp;
    [SerializeField] private Image button;
    [SerializeField] private TMP_Text buttonText;

    private Sprite[] stampSprite = new Sprite[5];

    //StageClearTimer Timer = new StageClearTimer();
    //SkillShield maruShield = new SkillShield();

    void Start()
    {
        canNextStage = false;

        if (KeyData.isMaruPad)
            selectKey = KeyCode.Joystick1Button0;
        else
            selectKey = KeyCode.Z;

        for (int i = 0; i < stampSprite.Length; i++)
            stampSprite[i] = Resources.Load<Sprite>("UI/ClearUI/ClearUIFin/UI_StageClear_Stamp_" + i);

        Timer.IsEnd = true;
        clearTime = (int)Timer.CurrentTime;
        clearTimeText[1].text = SetTime(clearTime);

        maruScore[0] = (int)Sword.totalDamage;
        maruScore[1] = maruShield.SkillDefence;
        maruScore[2] = playerMaru.cLife;

        nabiScore[0] = (int)Bullet.totalDamage;
        nabiScore[1] = playerNabi.NabiTraitScore;
        nabiScore[2] = playerNabi.cLife;

        for (int i = 0; i < maruLife.Length; i++)
        {
            if (i < maruScore[2])
                maruLife[i].sprite = lifeSprite[0];
            else
                maruLife[i].sprite = lifeSprite[1];

            if (i < nabiScore[2])
                nabiLife[i].sprite = lifeSprite[0];
            else
                nabiLife[i].sprite = lifeSprite[1];
        }

        ScoreCalculate();

        maruText[1].text = maruScore[0].ToString();
        maruText[3].text = maruScore[1].ToString();

        nabiText[1].text = nabiScore[0].ToString();
        nabiText[3].text = nabiScore[1].ToString();

        maruStamp.sprite = stampSprite[maruScore[4]];
        nabiStamp.sprite = stampSprite[nabiScore[4]];

        StartCoroutine("ClearUISet");
    }
    
    void Update()
    {
        if (canNextStage)
        {
            if (Input.GetKeyDown(selectKey))
            {
                SceneManager.LoadScene("StageSelectionScene");
            }
        }
    }

    private string SetTime(int _time)
    {
        string min = (_time / 60).ToString();
        if (int.Parse(min) < 10) min = "0" + min;
        string sec = (_time % 60).ToString();
        if (int.Parse(sec) < 10) sec = "0" + sec;

        return min + ":" + sec;
    }

    private void ScoreCalculate()
    {
        if (clearTime <= 325)
            timeScore = 112000;
        else if (clearTime > 325 && clearTime <= 345)
            timeScore = 104250;
        else if (clearTime > 345 && clearTime <= 355)
            timeScore = 96500;
        else if (clearTime > 355 && clearTime <= 365)
            timeScore = 88750;
        else if (clearTime > 365 && clearTime <= 385)
            timeScore = 81000;
        else if (clearTime > 385 && clearTime <= 395)
            timeScore = 73250;
        else if (clearTime > 395 && clearTime <= 405)
            timeScore = 65500;
        else if (clearTime > 405 && clearTime <= 425)
            timeScore = 57750;
        else if (clearTime > 425 && clearTime <= 435)
            timeScore = 50000;
        else if (clearTime > 435 && clearTime <= 475)
            timeScore = 44290;
        else if (clearTime > 475 && clearTime <= 505)
            timeScore = 38575;
        else if (clearTime > 505 && clearTime <= 535)
            timeScore = 32860;
        else if (clearTime > 535 && clearTime <= 565)
            timeScore = 27145;
        else if (clearTime > 565 && clearTime <= 595)
            timeScore = 21430;
        else if (clearTime > 595 && clearTime <= 625)
            timeScore = 15715;
        else
            timeScore = 10000;

        maruScore[3] = (maruScore[2] * 14040) + maruScore[0] + (maruScore[1] * 15060) + timeScore;
        nabiScore[3] = (nabiScore[2] * 14040) + nabiScore[0] + (nabiScore[1] / 10 * 15060) + timeScore;

        maruScore[4] = RankCalculate(maruScore[3]);
        nabiScore[4] = RankCalculate(nabiScore[3]);
    }

    private int RankCalculate(int _total)
    {
        if (_total < classJudgement[0])
            return 5;
        else if (_total >= classJudgement[0] && _total < classJudgement[1])
            return 4;
        else if (_total >= classJudgement[1] && _total < classJudgement[2])
            return 3;
        else if (_total >= classJudgement[2] && _total < classJudgement[3])
            return 2;
        else
            return 1;
    }

    private IEnumerator ClearUISet()
    {
        backGround.DOFade(1, 0.5f);
        yield return new WaitForSeconds(1f);
        totalTag.transform.DOLocalMoveY(totalTagPosY, 0.5f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.5f);
        maruPannel.transform.DOLocalMoveY(pannelPosY, 0.5f);
        nabiPannel.transform.DOLocalMoveY(pannelPosY, 0.5f);
        yield return new WaitForSeconds(0.5f);
        maruTag.transform.DOLocalMoveY(playerTagPosY, 0.5f);
        nabiTag.transform.DOLocalMoveY(playerTagPosY, 0.5f);
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < clearTimeText.Length; i++)
            clearTimeText[i].DOFade(1, 0.5f);
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < maruText.Length; i++)
        {
            maruText[i].DOFade(1, 0.5f);
            nabiText[i].DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.25f);
        }

        for (int i = 0; i < maruLife.Length; i++)
        {
            maruLife[i].DOFade(1, 0.5f);
            nabiLife[i].DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.25f);
        }

        maruStamp.DOFade(1, 0.1f);
        nabiStamp.DOFade(1, 0.1f);
        maruStamp.transform.DOScale(Vector3.one * 2.5f, 0.5f).SetEase(Ease.InExpo).From();
        nabiStamp.transform.DOScale(Vector3.one * 2.5f, 0.5f).SetEase(Ease.InExpo).From();
        yield return new WaitForSeconds(1f);

        button.DOFade(1, 0.5f);
        buttonText.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        canNextStage = true;
    }
}
