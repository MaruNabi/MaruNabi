using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVectorManager
{
    private int vertical;
    private int horizontal;

    public Vector2 GetDirectionalInputNabi()
    {
        if (KeyData.isNabiPad && !KeyData.isBothPad)
        {
            horizontal = ((Input.GetAxis("Horizontal_J1") > 0) ? 1 : 0) - ((Input.GetAxis("Horizontal_J1") < 0) ? 1 : 0);
            vertical = ((Input.GetAxis("Vertical_J1") > 0) ? 1 : 0) - ((Input.GetAxis("Vertical_J1") < 0) ? 1 : 0);
        }
        else if (KeyData.isBothPad)
        {
            horizontal = ((Input.GetAxis("Horizontal_J2") > 0) ? 1 : 0) - ((Input.GetAxis("Horizontal_J2") < 0) ? 1 : 0);
            vertical = ((Input.GetAxis("Vertical_J2") > 0) ? 1 : 0) - ((Input.GetAxis("Vertical_J2") < 0) ? 1 : 0);
        }
        else if (!KeyData.isNabiPad)
        {
            horizontal = (Input.GetKey(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.RightArrow)) && Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
            vertical = (Input.GetKey(KeyCode.UpArrow) && !(Input.GetKey(KeyCode.DownArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.UpArrow)) && Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        }

        return new Vector2(horizontal, vertical);
    }

    public Vector2 GetDirectionalInputMaru()
    {
        if (KeyData.isMaruPad)
        {
            horizontal = ((Input.GetAxis("Horizontal_J1") > 0) ? 1 : 0) - ((Input.GetAxis("Horizontal_J1") < 0) ? 1 : 0);
            vertical = ((Input.GetAxis("Vertical_J1") > 0) ? 1 : 0) - ((Input.GetAxis("Vertical_J1") < 0) ? 1 : 0);
        }
        else
        {
            horizontal = (Input.GetKey(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.RightArrow)) && Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
            vertical = (Input.GetKey(KeyCode.UpArrow) && !(Input.GetKey(KeyCode.DownArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.UpArrow)) && Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        }

        return new Vector2(horizontal, vertical);
    }
}
