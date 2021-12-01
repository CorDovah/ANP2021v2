using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordEnemy_Behaviour : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] int movSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        transform.DOMove(player.transform.position, movSpeed);
    }

    void Attack()
    {

    }
}
