using System.Collections;
using System.Collections.Generic;
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
    private Transform[] scholarTransformArr = new Transform[6];

    private GameObject[] scholars = new GameObject[6];

    private int mouseIdx;

    private bool isScholarBehit;

    private void Awake()
    {
        isScholarBehit = false;
    }
    void Start()
    {
        this.RandomScholars(); 
    }

    void Update()
    {

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

    public Vector3 RandomAttack()
    {
        int idx = Random.Range(0, 5);

        return scholarTransformArr[idx].position;
    }

    private int GetRandomIdx()
    {
        int idx = Random.Range(0, 5);

        return idx;
    }

    private void RandomScholars()
    {
        this.mouseIdx = GetRandomIdx();

        for(int i = 0; i < 6; i++)
        {
            if(i == this.mouseIdx)
            {
                Debug.Log("mouse : " + i);

                scholars[i] = CreateScholar(i, true);
            }
            else
            {
                scholars[i] = CreateScholar(i, false);
            }
        }
    }

    private GameObject CreateScholar(int idx, bool isMouse)
    {
        GameObject scholarGO = GameObject.Instantiate(scholarPrefab, transform);
        scholarGO.transform.position = scholarTransformArr[idx].position;

        StartCoroutine(CloudEffect(scholarTransformArr[idx].position));

        scholarGO.AddComponent<Scholar>();

        if (isMouse == true)
        {
            Scholar mouseScholar = scholarGO.GetComponent<Scholar>();
            mouseScholar.IsMouse = true;

            StartCoroutine(StrawEffect(scholarTransformArr[idx].position));
        }

        return scholarGO;
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
}
