using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordEnemy_Behaviour : MonoBehaviour
{
    Player_Behaviour player;
    WinCondition winScript;
    [SerializeField] int movSpeed;

    [SerializeField] Transform playerPos_;
    Collider2D _collider;
    Rigidbody2D rb;
    public Animator anim;

    public float attackRange;
    public bool Attacking;
    public bool Attack;
    public bool canMove;
    public GameObject sword1;

    [Header("Life")]
    public int life = 1;
    public bool isDead;

    Vector3 rotateLeft, rotateRight;

    void Start()
    {
        isDead = false;
        canMove = true;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        Attacking = true;
        canMove = true;
        rotateLeft = new Vector3(-0.45f, 0.5f);
        rotateRight = new Vector3(0.45f, 0.5f);
    }

    private void Awake()
    {
        player = FindObjectOfType<Player_Behaviour>();
        winScript = FindObjectOfType<WinCondition>();
        sword1.SetActive(false);
    }

    void Update()
    {
        playerPos_ = player.gameObject.transform;
        float distToPlayer = Vector2.Distance(transform.position, playerPos_.position);

        if (life == 0)
        {
            StartCoroutine(Dead());
        }

        if (Attacking && distToPlayer > attackRange)
        {
            ChasePlayer();
        }
        else if (Attacking && distToPlayer <= attackRange)
        {
            anim.SetBool("Walk", false);
            Attack = true;
            _Attack();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            life -= 1;
        }
    }

    void ChasePlayer()
    {
        if (transform.position.x < playerPos_.position.x && canMove)
        {
            anim.SetBool("Walk", true);
            transform.localScale = rotateRight;
            rb.velocity = new Vector2(movSpeed, 0f);
        }
        else if (transform.position.x > playerPos_.position.x && canMove)
        {
            anim.SetBool("Walk", true);
            transform.localScale = rotateLeft;
            rb.velocity = new Vector2(-movSpeed, 0f);
        }
        else
            anim.SetBool("Walk", false);
    }

    void _Attack()
    {
        if (Attack)
        {
            anim.SetTrigger("Attack");
        } 
    }

    public void ActivateSwordColliderEnemy()
    {
        sword1.SetActive(true);
    }

    public void DeactivateSwordColliderEnemy()
    {
        sword1.SetActive(false);
    }

    IEnumerator Dead()
    {
        winScript.restCounter(1);
        anim.SetBool("Death", true);
        canMove = false;
        _collider.enabled = false;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
