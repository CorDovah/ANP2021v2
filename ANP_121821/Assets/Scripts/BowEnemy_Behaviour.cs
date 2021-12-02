using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BowEnemy_Behaviour : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    Player_Behaviour player;
    WinCondition winScript;
    Bow bow;
    public Animator anim;
    public int life;
    public bool canMove;
    public float attackRange;
    Collider2D _collider;
    Rigidbody2D rb;

    private void Start()
    {
        life = 1;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player_Behaviour>();
        winScript = FindObjectOfType<WinCondition>();
        bow = FindObjectOfType<Bow>();
    }

    private void Update()
    {
        playerPos = player.gameObject.transform;
        LookAtPlayer();

        float distToPlayer = Vector2.Distance(this.transform.position, player.transform.position);
        //Debug.Log("Distance: " + distToPlayer);

        if (distToPlayer <= attackRange)
            Shooting();
        else
            StopCoroutine("ShootAgain");

        if (life == 0)
        {
            StartCoroutine(Dead());
        }
    }

    void LookAtPlayer()
    {
        if (transform.position.x < playerPos.position.x)
        {
            transform.localScale = new Vector2(0.45f, 0.5f);
        }
        else if (transform.position.x > playerPos.position.x)
        {
            transform.localScale = new Vector2(-0.45f, 0.5f);
        }
    }

    public void Shooting()
    {
        anim.SetTrigger("Attack");
        StartCoroutine("ShootAgain");
    }

    public void Shoot()
    {
        bow.Shoot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            life -= 1;
        }
    }

    IEnumerator ShootAgain()
    {
        yield return new WaitForSeconds(Random.Range(5.0f, 15.0f));
        Shooting();
    }

    IEnumerator Dead()
    {
        //anim.SetBool("Death", true);
        winScript.restCounter(1);
        canMove = false;
        _collider.enabled = false;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
