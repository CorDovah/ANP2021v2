using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Player_Behaviour player;
    SwordEnemy_Behaviour swEnemy;

    Collider2D _collider;
    Rigidbody2D rb;

    public int life = 1;
    public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        player = FindObjectOfType<Player_Behaviour>();
        swEnemy = FindObjectOfType<SwordEnemy_Behaviour>();

        isDead = false;
        life = 1;
    }

    void Update()
    {

        if (life == 0)
        {
            StartCoroutine(Dead());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            life -= 1;
        }
    }

    IEnumerator Dead()
    {
        swEnemy.canMove = false;
        isDead = true;
        _collider.enabled = false;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
