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

                scholars[i] = CreateScholar(i, false);

                scholars[i].AddComponent<Scholar>(); // TO DO : mouse·Î º¯°æ
            }
            else
            {
                scholars[i] = CreateScholar(i, true);
            }
        }
    }

    private GameObject CreateScholar(int idx, bool isScholar)
    {
        GameObject scholarGO = GameObject.Instantiate(scholarPrefab, transform);
        scholarGO.transform.position = scholarTransformArr[idx].position;

        StartCoroutine(CloudEffect(scholarTransformArr[idx].position));

        if (isScholar == true)
            scholarGO.AddComponent<Scholar>();

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
