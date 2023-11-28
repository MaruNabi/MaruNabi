using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public partial class Scholar : LivingEntity
{
    private SpriteRenderer scholarSpriteRenderer;

    private Color fadeColor;

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
}
