using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class ScholarManager : MonoBehaviour
{
    [SerializeField] private GameObject scholarPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform[] monsterTransformArr = new Transform[6];
    
    private GameObject[] scholars;
    private Sequence sequence;
    private const int INIT_MONSTERNUM = 6;
    private int roundNum;
    private bool isRoundEnd;
    private bool isStageClear;

    void Start()
    {
        roundNum = -1;
        scholars = new GameObject[6];

        Mouse.OnRoundEnd += RoundRestart;
        Mouse.OnDeath += StageClear;
        
        MakeMonsterRandomLocation(INIT_MONSTERNUM);
    }
    
    private void OnDestroy()
    {
        Mouse.OnRoundEnd -= RoundRestart;
        Mouse.OnDeath -= StageClear;
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
        if (inMonsterNum > 6)
            inMonsterNum = 6;
        
        List<EMonsterName> combination = new List<EMonsterName>();

        int mouseNum = 1;
        int scholarNum = inMonsterNum - mouseNum;
        int emptyNum = 6 - inMonsterNum;

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
        GameObject monsterGO = Instantiate(scholarPrefab, transform);
        monsterGO.transform.position = monsterTransformArr[idx].position;
        
        if (isMouse == false)
        {
            monsterGO.AddComponent<Scholar>();
        }
        else if (isMouse)
        {
            monsterGO.AddComponent<Mouse>();
        }

        return monsterGO;
    }

    private void StageClear()
    {
        Debug.Log("클리어");
    }
}