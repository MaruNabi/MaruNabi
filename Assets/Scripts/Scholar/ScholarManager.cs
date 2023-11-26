using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject scholarPrefab;

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
        GameObject go = GameObject.Instantiate(scholarPrefab);
        go.transform.position = scholarTransformArr[idx].position;

        if(isScholar == true)
            go.AddComponent<Scholar>();

        return go;
    }
}
