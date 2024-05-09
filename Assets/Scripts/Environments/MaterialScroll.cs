using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialScroll : ScrollManager
{
    [SerializeField] float speed;
    private float offset_x;
    private Renderer scrollRenderer;

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
