﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target;
    public float speed;
    public bool MoveRight;
    public Animator animator;
    public float stayTime;
    public float attackTime;
    public bool underAttack;
    public bool backAttack;
    public float invincibleTime;
    public int Hp;
    public int GetExp;
    private float attackDistance;

    //길찾기 구현하기
    private Vector2 destinationPosition; // 목적지
    //private PathFindingManager pathFindingManager;
    //public GameController enemyobj;

    public GameObject enemy;
    [SerializeField] private PlayerController player;
    public GameController gameController;
    [SerializeField] private WeaponBoard weaponBoard;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>();
        gameController = FindObjectOfType<GameController>();
        weaponBoard = GameObject.Find("UICanvus").transform.GetChild(0).GetComponentInChildren<WeaponBoard>();
    }
    void OnEnable()
    {
        //enemyobj = FindObjectOfType<GameController>();
        //pathFindingManager = FindObjectOfType<PathFindingManager>();
        destinationPosition = transform.position;
        attackTime = 0f;
        stayTime = 0f;
        Hp = 5;
        GetExp = 1;
        MoveRight = true;
    }
    public void FixedUpdate()
    {
        // target이 있을때
        if (target != null)
        {
            //pathFindingManager.PathFinding();

            // 목적지에 타겟 포지션 넣어줌
            destinationPosition = target.position;

            // player와의 거리
            attackDistance = Vector2.Distance(transform.position, target.position);
            if (attackDistance > 1f)
            {
                //pathFindingManager.PathFinding();

                transform.position = Vector2.MoveTowards(transform.position, destinationPosition, Time.fixedDeltaTime * speed);
                animator.SetFloat("Speed", 1f);

            }
            else if (attackDistance <= 1f)
            {
                if (attackTime > 2f)
                {
                    animator.SetFloat("Speed", 0f);
                    animator.SetTrigger("Attack");
                    player.Hp--;
                    player.PlayerStateSet();
                    attackTime = 0f;
                }
            }
            attackTime += Time.deltaTime;
            FlipFacing();
        }

        //target이 없을때
        else
        {
            // enemy의 위치와 목적지의 거리가 0.1보다 작으면
            if (Vector2.Distance(transform.position, destinationPosition) < 0.1f)
            {
                stayTime += Time.deltaTime;
                animator.SetFloat("Speed", 0f);

                if (stayTime > 2f)
                {
                    //랜덤으로 목적지 위치 생성
                    SetRedirection();
                    stayTime = 0;
                }
            }
            else
            {
                animator.SetFloat("Speed", 1f);
            }
            //enemy의 위치 = 직선으로 현재위치에서 목적지까지 거리이동
            transform.position = Vector2.MoveTowards(transform.position, destinationPosition, Time.fixedDeltaTime * speed);
        }
        if (underAttack)
        {
            invincibleTime += Time.deltaTime;
            
        }
        if (invincibleTime > 1f)
        {
            underAttack = false;
            invincibleTime = 0;
        }

    }

    private void SetRedirection()
    {
        // 목적지 = 랜덤위치 값
        destinationPosition = new Vector2(Random.Range(-15f, 15f), Random.Range(9f, 17f));
        FlipFacing();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "wall":
                SetRedirection();
                break;
            case "Skill":
                if (!underAttack)
                {
                    switch (gameController.damageType)
                    {
                        case "skill01":
                            OnDamage(3);
                            Debug.Log("skill01");
                            break;
                    }
                    underAttack = true;
                }
                break;
        }
    }

    public void OnDamage(int damage)
    {
        Hp -= damage;
        animator.SetTrigger("UnderAttack");

        if (Hp <= 0)
        {
            enemy.SetActive(false);
            gameController.CreateCoin();
            gameController.CreateItem();
            weaponBoard.ExpUp();
        }
        if (!backAttack)
        {
            if (MoveRight)
            {
                enemy.transform.position = new Vector2(transform.position.x - 1, transform.position.y);

            }
            else
            {
                enemy.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
        }
        else
        {
            if (MoveRight)
            {
                enemy.transform.position = new Vector2(transform.position.x + 1, transform.position.y);

            }
            else
            {
                enemy.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
        }
     
    }

    // 보는 방향 바꿔주기
    void FlipFacing()
    {
        if (destinationPosition.x < transform.position.x)
        {
            MoveRight = false;
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            MoveRight = true;
            transform.localScale = new Vector2(1, 1);

        }
    }

}
