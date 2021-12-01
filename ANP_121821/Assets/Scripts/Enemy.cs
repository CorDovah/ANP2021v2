using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int life = 1;
    bool isDead;

    void Start()
    {

    }

    void Update()
    {
        if (life == 0)
        {
            StartCoroutine(Dead());
        }
    }

    void Shoot()
    {

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

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
