using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private int deleteTime;

    void Start()
    {
        Invoke("Destroy", deleteTime);
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
