using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletVectorManager : MonoBehaviour
{
    protected static Vector2 bulletVector;

    void Start()
    {

    }

    void Update()
    {
        Vector2 directionalInput = GetDirectionalInput();

        if (directionalInput.magnitude > 0)
        {
            float angleX = directionalInput.x;
            float angleY = directionalInput.y;

            bulletVector = new Vector2(angleY, angleX);
            
            transform.rotation = Quaternion.Euler(bulletVector);
        }
    }
    Vector2 GetDirectionalInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        return new Vector2(horizontalInput, verticalInput);
    }
}
