using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowEnemy_Behaviour : MonoBehaviour
{
    Player_Behaviour player;
    Transform playerPos_;

    void Start()
    {

    }

    private void Awake()
    {
        player = FindObjectOfType<Player_Behaviour>();
    }

    private void Update()
    {
        playerPos_ = player.gameObject.transform;
        
    }

    void LookAtPlayer()
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

}
