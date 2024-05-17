using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ScholarManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private NextStageWall nextStageWall;
    [SerializeField] private GameObject scholarPrefab;
    [SerializeField] private GameObject tree;
    [SerializeField] private GameObject wall;
    [SerializeField] private Transform[] monsterTransformArr = new Transform[7];
    [SerializeField] private Transform spawnTrasnform;
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;


    private bool stage1Start = true;

    public bool Stage1Start
    {
        set => stage1Start = value;
    }

    private const int INIT_MONSTERNUM = 7;
    private int roundNum;
    private float mouseHp;
    private GameObject[] scholars;
    private Sequence sequence;
    private bool isRoundEnd;
    private bool isStageClear;

    private void Start()
    {
        MouseScholar.OnRoundEnd += RoundRestart;
        MouseScholar.StageClear += StageClearProduction;
        Entity.AttackEvent += DamageAllPlayers;

        mouseHp = Utils.GetDictValue(Managers.Data.monsterDict, "MOUSESCHOLAR_MONSTER").LIFE;

        if (stage1Start)
        {
            roundNum = -1;
            scholars = new GameObject[INIT_MONSTERNUM];

            MakeMonsterRandomLocation(INIT_MONSTERNUM);
        }
    }

    private void OnDestroy()
    {
        MouseScholar.OnRoundEnd -= RoundRestart;
        MouseScholar.StageClear -= StageClearProduction;
        Entity.AttackEvent -= DamageAllPlayers;
    }

    public void DamageAllPlayers()
    {
        DamageDelay().Forget();
    }

    private async UniTaskVoid DamageDelay()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
        player1.PlayerHitSpecial(player1.transform.position);
        player2.PlayerHitSpecial(player2.transform.position);
    }

    private void RoundRestart(float _hp)
    {
        mouseHp = _hp;
        MakeMonsterRandomLocation(INIT_MONSTERNUM);
    }

    private void MakeMonsterRandomLocation(int inMonsterNum)
    {
        // 여기서 라운드 시작 취급
        roundNum++;

        List<EMonsterName> monsters = GetRandomCombination(inMonsterNum);

        for (int i = 0; i < monsters.Count; i++)
        {
            EMonsterName eMonster = monsters[i];
            switch (eMonster)
            {
                case EMonsterName.Mouse:
                    scholars[i] = CreateMonster(i, true);
                    break;
                case EMonsterName.Scholar:
                    scholars[i] = CreateMonster(i, false);
                    break;
                case EMonsterName.Empty:
                    break;
            }
        }
    }

    private List<EMonsterName> GetRandomCombination(int inMonsterNum)
    {
        if (inMonsterNum > INIT_MONSTERNUM)
            inMonsterNum = INIT_MONSTERNUM;

        List<EMonsterName> combination = new List<EMonsterName>();

        int mouseNum = 1;
        int scholarNum = inMonsterNum - mouseNum;
        int emptyNum = INIT_MONSTERNUM - inMonsterNum;

        // 입력받은 개수만큼 리스트에 추가
        combination.AddRange(Enumerable.Repeat(EMonsterName.Mouse, mouseNum));
        combination.AddRange(Enumerable.Repeat(EMonsterName.Scholar, scholarNum));
        combination.AddRange(Enumerable.Repeat(EMonsterName.Empty, emptyNum));

        // 섞기
        for (int i = 0; i < combination.Count; i++)
        {
            int temp = (int)combination[i];
            int randomIndex = Random.Range(i, combination.Count);
            combination[i] = combination[randomIndex];
            combination[randomIndex] = (EMonsterName)temp;
        }

        return combination;
    }

    private GameObject CreateMonster(int idx, bool isMouse)
    {
        GameObject monsterGO = Instantiate(scholarPrefab, spawnTrasnform);
        monsterGO.transform.position = monsterTransformArr[idx].position;

        if (isMouse == false)
        {
            monsterGO.AddComponent<Scholar>();
        }
        else if (isMouse)
        {
            monsterGO.AddComponent<MouseScholar>().InitHp(mouseHp);
        }

        return monsterGO;
    }

    private void StageClearProduction(GameObject _mouseScholar)
    {
        player1.PlayerStateTransition(false, 0);
        player2.PlayerStateTransition(false, 0);
        
        virtualCamera.m_Follow = _mouseScholar.transform;
        virtualCamera.m_LookAt = _mouseScholar.transform;
        virtualCamera.gameObject.SetActive(true);

        ProductionWait(_mouseScholar).Forget();
    }

    async UniTaskVoid ProductionWait(GameObject _mouseScholar)
    {
        for (int i = 0; i < scholars.Length; i++)
        {
            var item = scholars[i];
            if (item != _mouseScholar)
            {
                Utils.GetOrAddComponent<Scholar>(item).StopStateMachine();
            }
        }
        
        await UniTask.Delay(TimeSpan.FromSeconds(2f));
        MouseScholar.PunishProduction?.Invoke();

        await UniTask.Delay(TimeSpan.FromSeconds(2.3f));

        for (var index = 0; index < scholars.Length; index++)
        {
            var item = scholars[index];
            if (item == _mouseScholar)
            {
                Utils.GetOrAddComponent<MouseScholar>(_mouseScholar).JumpAnimation(_mouseScholar.transform.position);
            }
        }

        await UniTask.Delay(TimeSpan.FromSeconds(3f));

        player1.PlayerStateTransition(true, 0);
        player2.PlayerStateTransition(true, 0);
        virtualCamera.gameObject.SetActive(false);

        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        tree.transform.DOMoveX(-28f, 1.8f);
        wall.transform.DOMoveX(-25f, 1.8f);
        nextStageWall.isStage1Clear = true;
    }
}