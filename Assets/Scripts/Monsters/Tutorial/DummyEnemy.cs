using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{
    [SerializeField] private int dummyHp;
    private SpriteRenderer dummySpriteRenderer;

    void Start()
    {
        dummySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (dummyHp <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            StartCoroutine(DummyHit());
        }
    }

    private IEnumerator DummyHit()
    {
        dummyHp -= 1;
        dummySpriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        dummySpriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
