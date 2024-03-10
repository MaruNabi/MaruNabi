using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class ScholarManager : MonoBehaviour
{
    [SerializeField]
    private GameObject scholarPrefab;

    [SerializeField]
    private GameObject cloudPrefab;

    [SerializeField]
    private GameObject strawPrefab;

    [SerializeField]
    public GameObject fanPrefab;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private Transform[] monsterTransformArr = new Transform[6];

    private GameObject[] scholars = new GameObject[6];

    private bool isScholarBehit;
    private bool isInit = true;
    private bool isFanEnd;

    // TO DO : 20240225 kimyeonmo 기획에 따라 달라질 수 있는 수치는 데이터에셋등 다른 형태로 저장하기
    private const int initMonsterNum = 6;
    private int deathMonster = 0;

    enum MonsterType
    {
        Mouse = 0,      // 쥐
        Scholar = 1,  // 선비
        Empty = 2     // 빈칸
    }

    public bool IsInit
    {
        get { return isInit; }
        set { isInit = value; }
    }

    public bool IsFanEnd
    {
        get { return isFanEnd; }
        set { isFanEnd = value; }
    }

    private void Awake()
    {
        isScholarBehit = false;
    }
    void Start()
    {
        this.MakeRandomMonsterLocation(initMonsterNum);
    }

    void Update()
    {
        if(this.isFanEnd == true)
        {
            this.MakeRandomMonsterLocation(GetArriveMonster());
            this.isFanEnd = false;
        }
    } 

    public void SetSchloarBehit(bool isHit)
    {
        this.isScholarBehit = isHit;
    }

    public bool GetIsSchloarBehit()
    {
        return this.isScholarBehit;
    }

    public GameObject GetPlayer()
    {
        return this.player;
    }

    static List<MonsterType> GetRandomCombination(int inMonsterNum) // inMonsterNum = 생존한 몬스터 수(선비 + 쥐)
    {
        List<MonsterType> combination = new List<MonsterType>();

        int mouseNum = 1;
        int scholarNum = inMonsterNum - mouseNum;
        int emptyNum = 6 - inMonsterNum;

        combination.AddRange(Enumerable.Repeat(MonsterType.Mouse, mouseNum));
        combination.AddRange(Enumerable.Repeat(MonsterType.Scholar, scholarNum));
        combination.AddRange(Enumerable.Repeat(MonsterType.Empty, emptyNum));

        for(int i = 0; i < combination.Count; i++)
        {
            int temp = (int)combination[i];
            int randomIndex = Random.Range(i, combination.Count);
            combination[i] = combination[randomIndex];
            combination[randomIndex] = (MonsterType)temp;
        }

        return combination;
    }


    public void MakeRandomMonsterLocation(int inMonsterNum)
    {
        List<MonsterType> monsters = GetRandomCombination(inMonsterNum);

        for (int i = 0; i < monsters.Count; i++)
        {
            MonsterType monster = monsters[i];
            switch (monster) 
            {
                case MonsterType.Mouse:
                    Debug.Log("Mouse : " + i);
                    scholars[i] = CreateMonster(i, true);
                    break;

                case MonsterType.Scholar:
                    Debug.Log("Scholar : " + i);
                    scholars[i] = CreateMonster(i, false);
                    break;

                case MonsterType.Empty:
                    Debug.Log("Empty : " + i);
                    break;

                default: break;
            }
        }
    }

    private GameObject CreateMonster(int idx, bool isMouse)
    {
        GameObject monsterGO = GameObject.Instantiate(scholarPrefab, transform);
        monsterGO.transform.position = monsterTransformArr[idx].position;

        StartCoroutine(CloudEffect(monsterTransformArr[idx].position));

        if(isMouse == false)
        {
            monsterGO.AddComponent<Scholar>();
        }
        else if (isMouse == true)
        {
            monsterGO.AddComponent<Mouse>();
            // Scholar mouseScholar = scholarGO.GetComponent<Scholar>();
            // mouseScholar.IsMouse = true;

            StartCoroutine(StrawEffect(monsterTransformArr[idx].position));
        }

        return monsterGO;
    }

    private Color fadeColor;

    public IEnumerator CloudEffect(Vector3 cloudPosition)
    {
        GameObject cloud = GameObject.Instantiate(cloudPrefab);
        cloud.transform.position = cloudPosition;

        SpriteRenderer cloudSpriteRenderer = cloud.GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(0.3f);

        while (this.fadeColor.a >= 0f)
        {
            this.fadeColor = cloudSpriteRenderer.color;
            this.fadeColor.a -= 0.2f;
            cloudSpriteRenderer.color = this.fadeColor;

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(cloud);
    }

    public IEnumerator StrawEffect(Vector3 strawPosition)
    {
        GameObject straw = GameObject.Instantiate(strawPrefab);
        straw.transform.position = strawPosition;

        yield return new WaitForSeconds(1.0f);

        Destroy(straw);
    }

    public GameObject MakeFan(Vector3 fanPosition)
    {
        GameObject fan = GameObject.Instantiate(fanPrefab);
        fan.transform.position = fanPosition;

        return fan;
    }

    public void DestroyFan(GameObject fan)
    {
        Destroy(fan);
    }

    public int GetArriveMonster()
    {
        return 6 - deathMonster;
    }

    public void SetDeathMonster()
    {
        deathMonster++;
    }
}
