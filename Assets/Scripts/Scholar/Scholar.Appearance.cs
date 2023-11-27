using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Scholar : LivingEntity
{
    private SpriteRenderer scholarSpriteRenderer;

    private Color FadeColor;

    private float fadeSpeed = 1.0f;

    public IEnumerator AppearanceCoroutine()
    {
        FadeColor = scholarSpriteRenderer.color;
        FadeColor.a = 0f;
        scholarSpriteRenderer.color = FadeColor;

        while (FadeColor.a <= 1f)
        {
            FadeColor = scholarSpriteRenderer.color;
            FadeColor.a += 0.1f;
            scholarSpriteRenderer.color = FadeColor;
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}
