using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public int counter;
    Player_Behaviour player;

    private void Start()
    {
        player = FindObjectOfType<Player_Behaviour>();
    }

    private void Update()
    {
        if(counter == 0)
        {
            player.hasWon = true;
        }

        if(player.hasWon)
        {
            player.StartCoroutine(player.Win());
        }
    }

    public void restCounter(int number)
    {
        counter = counter - number;
    }
}
