using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Match
{
    public List<Wrangler> wranglers;
    public Arena arena;
    public int enemyPower = 0;
    public int allyPower = 0;

    public void init()
    {
        arena.init();

        //holders ally
        Wrangler ally = wranglers[0];
        ally.handHolder = arena.allyHand;
        ally.handHolder.owner = ally;
        ally.cardHolders = arena.allyHolders;
        for (int i = 0; i < arena.allyHolders.Count; i++)
        {
            CardHolder ch = arena.allyHolders[i];
            ch.owner = ally;
        }

        //holders enemy
        Wrangler enemy = wranglers[1];
        enemy.handHolder = arena.enemyHand;
        enemy.handHolder.owner = enemy;
        enemy.cardHolders = arena.enemyHolders;
        for (int i = 0; i < arena.enemyHolders.Count; i++)
        {
            CardHolder ch = arena.enemyHolders[i];
            ch.owner = enemy;
        }
    }

    public void calculateScores()
    {
        enemyPower = 0;
        allyPower = 0;

        for (int i = 0; i < arena.data.lanes.Count; i++)
        {
            ArenaLane lane = arena.lanes[i];
            lane.calculatePower();
            allyPower += lane.allyPower;
            enemyPower += lane.enemyPower;
        }
        enemyPower = Mathf.Clamp(enemyPower, 0, enemyPower);
        allyPower = Mathf.Clamp(allyPower, 0, allyPower);
    }
}
