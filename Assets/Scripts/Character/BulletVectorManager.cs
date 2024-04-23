using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVectorManager
{
    private int vertical;
    private int horizontal;

    public Vector2 GetDirectionalInputNabi()
    {
        vertical = (Input.GetKey(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.RightArrow)) && Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        horizontal = (Input.GetKey(KeyCode.UpArrow) && !(Input.GetKey(KeyCode.DownArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.UpArrow)) && Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        return new Vector2(vertical, horizontal);
    }

    public Vector2 GetDirectionalInputMaru()
    {
        vertical = (Input.GetKey(KeyCode.D) && !(Input.GetKey(KeyCode.A)) ? 1 : 0) - (!(Input.GetKey(KeyCode.D)) && Input.GetKey(KeyCode.A) ? 1 : 0);
        horizontal = (Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.S)) ? 1 : 0) - (!(Input.GetKey(KeyCode.W)) && Input.GetKey(KeyCode.S) ? 1 : 0);

        return new Vector2(vertical, horizontal);
    }
}
