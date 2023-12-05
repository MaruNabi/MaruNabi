using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Scholar : LivingEntity
{
    private SpriteRenderer scholarSpriteRenderer;

    private Color fadeColor;

    private float transparentAlpha;
    private float transparent = 0.5f;
    private float normal = 1.0f;

    private Color BehitColor;

    public IEnumerator AppearanceCoroutine()
    {
        this.fadeColor = scholarSpriteRenderer.color;
        this.fadeColor.a = 0f;
        scholarSpriteRenderer.color = this.fadeColor;

        while (this.fadeColor.a <= 1f)
        {
            this.fadeColor = scholarSpriteRenderer.color;
            this.fadeColor.a += 0.15f;
            scholarSpriteRenderer.color = this.fadeColor;
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator BeHitEffect()
    {
        this.BehitColor = this.scholarSpriteRenderer.color;

        this.BehitColor.a = this.transparent;

        this.scholarSpriteRenderer.color = this.BehitColor;

        yield return new WaitForSeconds(0.5f);

        this.BehitColor.a = this.normal;

        this.scholarSpriteRenderer.color = this.BehitColor;
    }
}
