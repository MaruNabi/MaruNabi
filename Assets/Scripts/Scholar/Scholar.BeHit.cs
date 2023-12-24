using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class Scholar : LivingEntity
{
    private TMP_Text hpTextBox;

    public bool isIdle = false;

    public bool IsIdle
    {
        get { return isIdle; }
        set { isIdle = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if(this.IsIdle == true)
            {
                Debug.Log("총알 맞았다!!");

                scholarManager.SetSchloarBehit(true);
                OnDamage(523456789);

                Debug.Log("비히트?? " + scholarManager.GetIsSchloarBehit());

                this.hpTextBox.text = HP.ToString();

                StartCoroutine(BeHitEffect());
            }
        }
    }
}
