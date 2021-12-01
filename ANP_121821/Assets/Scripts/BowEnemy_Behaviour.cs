using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemy_Behaviour : MonoBehaviour
{
    Player_Behaviour player;
    [SerializeField] int movSpeed;

    [SerializeField] Transform playerPos_;
    Rigidbody2D rb;

    public float attackRange;
    public bool Attacking;
    public bool Attack;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Attacking = true;
    }

    private void Awake()
    {
        player = FindObjectOfType<Player_Behaviour>();
    }

    void Update()
    {
        playerPos_ = player.gameObject.transform;

        if (Attacking)
        {
            ChasePlayer();
        }
    }

    void ChasePlayer()
    {
        if (transform.position.x < playerPos_.position.x)
        {
            transform.localScale = new Vector2(0.45f, 0.5f);
        }
        else if (transform.position.x > playerPos_.position.x)
        {
            transform.localScale = new Vector2(-0.45f, 0.5f);
        }
    }

    void Shoot()
    {

    }
}
