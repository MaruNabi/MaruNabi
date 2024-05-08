using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class ScholarManager : MonoBehaviour
{
    [SerializeField] private bool roundStart;
    [SerializeField] private GameObject scholarPrefab;
    [SerializeField] private Transform[] monsterTransformArr = new Transform[7];
    [SerializeField] private Transform spawnTrasnform;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Player player;
    
    private GameObject[] scholars;
    private Sequence sequence;
    private const int INIT_MONSTERNUM = 7;
    private int roundNum;
    private bool isRoundEnd;
    private bool isStageClear;

    void Start()
    {
        if (roundStart)
        {
            roundNum = -1;
            scholars = new GameObject[INIT_MONSTERNUM];

            MouseScholar.OnRoundEnd += RoundRestart;
            MouseScholar.OnRoundClear += StageClear;
        
            MakeMonsterRandomLocation(INIT_MONSTERNUM);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (var VARIABLE in scholars)
            {
                if (VARIABLE.TryGetComponent<MouseScholar>(out MouseScholar mouseScholar))
                {
                    mouseScholar.StageSkip();
                }
            }
        }
    }

    private void OnDestroy()
    {
        MouseScholar.OnRoundEnd -= RoundRestart;
        MouseScholar.OnRoundClear -= StageClear;
    }

    private void RoundRestart()
    {
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
            monsterGO.AddComponent<MouseScholar>();
        }

        return monsterGO;
    }

    private void StageClear(GameObject _obj)
    {
        player.PlayerStateTransition(false);
        
        for (var index = 0; index < scholars.Length; index++)
        {
            var item = scholars[index];
            if (item == _obj)
            {
                GameObject monsterGO = Instantiate(scholarPrefab, spawnTrasnform);
                if (index == 0)
                {
                    monsterGO.transform.position = monsterTransformArr[1].position;
                }
                else
                {
                    monsterGO.transform.position = monsterTransformArr[index-1].position;
                }

                var scholar = Utils.GetOrAddComponent<Scholar>(monsterGO);
                StartCoroutine(scholar.SetStatePunish());
            }
        }

        // 연출
        Debug.Log("클리어");
        virtualCamera.m_Follow = _obj.transform;
        virtualCamera.m_LookAt = _obj.transform;
        virtualCamera.gameObject.SetActive(true);
    }
}