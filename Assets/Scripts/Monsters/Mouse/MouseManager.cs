using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    public GameObject fanPrefab;

    [SerializeField]
    private GameObject player;

    private bool isMouseBehit;
   
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetMouseBehit(bool isHit)
    {
        this.isMouseBehit = isHit;
    }

    public bool GetIsMouseBehit()
    {
        return this.isMouseBehit;
    }

    public GameObject GetPlayer()
    {
        return this.player;
    }

    public GameObject MakeFan(Vector3 fanPosition)
    {
        GameObject fan = GameObject.Instantiate(fanPrefab);
        fan.transform.position = fanPosition;

        return fan;
    }

    public void DestroyFan(GameObject fan)
    {
        Destroy(fan);
    }
}
