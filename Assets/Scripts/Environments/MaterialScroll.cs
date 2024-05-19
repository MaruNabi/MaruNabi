using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialScroll : ScrollManager
{
    private float offset_x;
    private Renderer scrollRenderer;
    [SerializeField] private bool isSkyMaterial;
    public bool IsSkyMaterial => isSkyMaterial;
    
    private void Start()
    {
        scrollRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (isStart)
        {
            UpdateScroll();
        }
    }

    private void UpdateScroll()
    {
        offset_x += speed * Time.deltaTime;
        Vector2 offset = new Vector2(offset_x, 0);
        scrollRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}
