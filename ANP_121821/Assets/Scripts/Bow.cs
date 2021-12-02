using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bow : MonoBehaviour
{
    public GameObject prefabBullet;
    Pool bulletPool;
    Player_Behaviour player;

    private void Start()
    {
        bulletPool = new Pool();
        bulletPool.Starting(prefabBullet, 5);
        player = FindObjectOfType<Player_Behaviour>();
    }

    public void Shoot()
    {
        GameObject go = bulletPool.Spawn(transform.position, transform.rotation);
        go.transform.DOMove(player.transform.position, 0.5f);
        go.transform.DORotate(new Vector3(0.0f, 0.0f, -45.0f), 0.2f);
    }
}
