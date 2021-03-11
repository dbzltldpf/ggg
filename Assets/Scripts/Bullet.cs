using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigid;
    protected EnemyController enemy;

    void Awake()
    {
        rigid = GetComponentInChildren<Rigidbody2D>();
        enemy = FindObjectOfType<EnemyController>();
    }

    void Update()
    {
        rigid.velocity = transform.right * 10f;
        if (transform.position.x >= 15f || transform.position.x <= -15f)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("명중");
            enemy.OnDamage(1);
            gameObject.SetActive(false);
        }
    }

}
