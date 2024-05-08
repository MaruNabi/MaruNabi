using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialScroll : ScrollManager
{
    [SerializeField] float speed;
    private float offset_x;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
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
        renderer.material.SetTextureOffset("_MainTex", offset);
    }

}
