using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BowEnemy_Behaviour : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    Player_Behaviour player;
    Bow bow;
    public Animator anim;
    public bool canMove;
    public float attackRange;

    private void Start()
    {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player_Behaviour>();
        bow = FindObjectOfType<Bow>();
    }

    private void Update()
    {
        playerPos = player.gameObject.transform;
        LookAtPlayer();

        float distToPlayer = Vector2.Distance(this.transform.position, player.transform.position);
        Debug.Log("Distance: " + distToPlayer);

        if (distToPlayer <= attackRange)
            Shooting();
        else
            StopCoroutine("ShootAgain");
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

    IEnumerator ShootAgain()
    {
        yield return new WaitForSeconds(Random.Range(3.0f, 10.0f));
        Shooting();
    }
}
