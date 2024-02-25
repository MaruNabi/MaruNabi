using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mouse : LivingEntity
{
    private SpriteRenderer mouseSpriteRenderer;

    private Color fadeColor;

    private float transparent = 0.5f;
    private float normal = 1.0f;

    private Color BehitColor;

    public IEnumerator AppearanceCoroutine()
    {
        this.fadeColor = mouseSpriteRenderer.color;
        this.fadeColor.a = 0f;
        mouseSpriteRenderer.color = this.fadeColor;

        while (this.fadeColor.a <= 1f)
        {
            this.fadeColor = mouseSpriteRenderer.color;
            this.fadeColor.a += 0.05f;
            mouseSpriteRenderer.color = this.fadeColor;

            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator BeHitEffect()
    {
        this.BehitColor = this.mouseSpriteRenderer.color;

        this.BehitColor.a = this.transparent;

        this.mouseSpriteRenderer.color = this.BehitColor;

        yield return new WaitForSeconds(0.3f);

        this.BehitColor.a = this.normal;

        this.mouseSpriteRenderer.color = this.BehitColor;
    }
}
