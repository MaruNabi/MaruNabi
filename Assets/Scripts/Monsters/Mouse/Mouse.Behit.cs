using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class Mouse : LivingEntity
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
            if (this.IsIdle == true)
            {
                Debug.Log("�Ѿ� �¾Ҵ�!!");

                mouseManager.SetMouseBehit(true);
                OnDamage(523456789);

                Debug.Log("����Ʈ?? " + mouseManager.GetIsMouseBehit());

                this.hpTextBox.text = HP.ToString();

                StartCoroutine(BeHitEffect());
            }
        }
    }
}
