using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVectorManager : MonoBehaviour
{
    public static Vector2 bulletVector;
    private int vertical;
    private int horizontal;

    void Update()
    {
        Vector2 directionalInput = GetDirectionalInput();

        if (directionalInput.magnitude > 0)
        {
            float angleX = directionalInput.x;
            float angleY = directionalInput.y;

            bulletVector = new Vector2(angleX, angleY);
        }

        else
        {
            bulletVector = new Vector2(0.0f, 0.0f);
        }
    }
    Vector2 GetDirectionalInput()
    {
        vertical = (Input.GetKey(KeyCode.RightArrow) && !(Input.GetKey(KeyCode.LeftArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.RightArrow)) && Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        horizontal = (Input.GetKey(KeyCode.UpArrow) && !(Input.GetKey(KeyCode.DownArrow)) ? 1 : 0) - (!(Input.GetKey(KeyCode.UpArrow)) && Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        return new Vector2(vertical, horizontal);
    }
}
