using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public EnemyController enemy;
    public PlayerController player;

    private void Awake()
    {
        enemy = FindObjectOfType<EnemyController>();
        player = FindObjectOfType<PlayerController>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (player.attacking)
            {
                Debug.Log("검");
                enemy.OnDamage(1);
            }
           
        }
    }
}
