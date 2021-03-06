using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningSystem : MonoBehaviour
{
    [Header("SpawnOptions")]
    [SerializeField] GameObject[] Enemies;
    public Vector3 SpawnPos;
    [SerializeField] bool spawnAgain;

    [Header("EnemyVariables")]
    public bool enemiesDead;
    public int enemiesKilled;
    public int enemiesAlive;

    private void Start()
    {
        SpawnPos = this.transform.position;
        StartCoroutine("SpawnEnemies");
    }

    void Update()
    {
        enemiesKilled = enemiesAlive - enemiesKilled;

        if (enemiesAlive <= 0)
        {
            enemiesDead = true;
        }
        else
            enemiesDead = false;


        if (enemiesDead && spawnAgain)
        {
            StartCoroutine("SpawnEnemies");
        }
    }

    IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        foreach (var enemy in Enemies)
        {
            Instantiate(enemy, SpawnPos, Quaternion.identity);
            yield return wait;
        }
    }
}
