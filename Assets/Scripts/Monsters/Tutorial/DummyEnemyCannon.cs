using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DummyEnemyCannon : MonoBehaviour
{
    private float cannonSetXPosition = 29.29f;
    private float curTime = 0.0f;

    private GameObject cannonBallPrefab;

    void Start()
    {
        cannonBallPrefab = Resources.Load<GameObject>("Prefabs/Tutorial/Tut_Bullet");
    }

    void OnEnable()
    {
        StartCoroutine(CannonInit());
    }

    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime > 1.5)
        {
            GameObject cannonBall = Instantiate(cannonBallPrefab);
            cannonBall.transform.position = transform.position;
            curTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator CannonInit()
    {
        transform.DOLocalMoveX(cannonSetXPosition, 2.0f);
        yield return new WaitForSeconds(3.0f);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
