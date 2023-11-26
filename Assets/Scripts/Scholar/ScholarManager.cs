using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScholarManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject scholarPrefab;

    [SerializeField]
    private Transform[] scholarTransformArr = new Transform[6];

    private Scholar scholar;
    private void Awake()
    {
        
    }
    void Start()
    {
        scholar = new Scholar(scholarPrefab, scholarTransformArr[0]);
    }

    void Update()
    {
        
    }
}
