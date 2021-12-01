using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player_Behaviour player;

    public int life = 1;
    bool isDead;

    void Start()
    {
        isDead = false;
        life = 1;
    }

    private void Awake()
    {
        player = FindObjectOfType<Player_Behaviour>();
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
        isDead = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
