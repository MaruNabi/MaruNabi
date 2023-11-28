using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject scholarPrefab;

    [SerializeField]
    private GameObject cloudPrefab;

    [SerializeField]
    private Transform[] scholarTransformArr = new Transform[6];

    private GameObject[] scholars = new GameObject[6];

    private int mouseIdx;

    private void Awake()
    {
        
    }
    void Start()
    {
        this.RandomScholars();
    }

    void Update()
    {
        
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
        GameObject scholarGO = GameObject.Instantiate(scholarPrefab);
        scholarGO.transform.position = scholarTransformArr[idx].position;

        StartCoroutine(CloudEffect(scholarTransformArr[idx].position));

        if (isScholar == true)
            scholarGO.AddComponent<Scholar>();

        return scholarGO;
    }

    private Color FadeColor;

    private IEnumerator CloudEffect(Vector3 cloudPosition)
    {
        GameObject cloud = GameObject.Instantiate(cloudPrefab);
        cloud.transform.position = cloudPosition;

        SpriteRenderer cloudSpriteRenderer = cloud.GetComponent<SpriteRenderer>();

        yield return new WaitForSeconds(0.3f);

        while (FadeColor.a >= 0f)
        {
            FadeColor = cloudSpriteRenderer.color;
            FadeColor.a -= 0.2f;
            cloudSpriteRenderer.color = FadeColor;

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(cloud);
    }
}
