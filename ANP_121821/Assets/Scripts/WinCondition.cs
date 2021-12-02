using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public int counter;
    Player_Behaviour player;

    private void Start()
    {
        counter = 5;
        player = FindObjectOfType<Player_Behaviour>();
    }

    private void Update()
    {
        if(counter == 0)
        {
            player.hasWon = true;
        }
    }

    public void restCounter(int number)
    {
        counter = counter - number;
    }
}
