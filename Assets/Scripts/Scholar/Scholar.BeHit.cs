using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class Scholar : LivingEntity
{
    private TMP_Text hpTextBox;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            scholarManager.SchloarBehit();
            OnDamage(123456789);
            Debug.Log(HP);
            this.hpTextBox.text = HP.ToString();

            StartCoroutine(BeHitEffect());
        }
    }
}
