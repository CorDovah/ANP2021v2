using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject player;

    public int life = 1;
    bool isDead;

    void Start()
    {
        isDead = false;
        life = 1;
    }

    void Update()
    {
        Vector3 dir = player.transform.position - transform.position;

        if(dir.x > transform.position.x)
        {
            turnRight();
        }
        else
        {
            turnLeft();
        }


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

    void turnLeft()
    {
        this.transform.localScale = new Vector3(-0.45f, 0.5f, 0f);
    }

    void turnRight()
    {
        this.transform.localScale = new Vector3(0.45f, 0.5f, 0f);
    }

    IEnumerator Dead()
    {
        isDead = true;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
