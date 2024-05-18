using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemyCannonBall : MonoBehaviour
{
    private Rigidbody2D cannonBallRigid;
    private Transform maruTransform;
    private Transform nabiTransform;

    [SerializeField] private Vector3 playerSpawnVec = new Vector3(89.05f, 0.0f, 0.0f);

    [SerializeField] private float speed;

    void Start()
    {
        cannonBallRigid = GetComponent<Rigidbody2D>();

        maruTransform = GameObject.Find("Maru_Test").GetComponent<Transform>();
        nabiTransform = GameObject.Find("Nabi_Test").GetComponent<Transform>();

        Invoke("DestroyCannon", 2.5f);
    }

    void Update()
    {
        cannonBallRigid.velocity = new Vector2(-speed, 0);
    }

    private void DestroyCannon()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.name == "MaruShild")
            {
                DestroyCannon();
            }
            else
            {
                if (collision.gameObject.name == "Maru_Test")
                {
                    maruTransform.position = playerSpawnVec;
                }
                else
                {
                    nabiTransform.position = playerSpawnVec;
                }
            }
        }
    }
}
