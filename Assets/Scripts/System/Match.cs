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
            int laneAlly = arena.allyHolders[i].cardList.Sum(card => card.data.power);
            int laneEnemy = arena.enemyHolders[i].cardList.Sum(card => card.data.power);
            CreatureCardData ally = arena.allyHolders[i].Card?.data;
            CreatureCardData enemy = arena.enemyHolders[i].Card?.data;
            if (!enemy)
            {
                allyPower += laneAlly;
            }
            if (!ally)
            {
                enemyPower += laneEnemy;
            }
            if (ally && enemy)
            {
                int _ap = ally.power;
                int _ep = enemy.power;
                if (arena.data.rpsSetData.beats(ally.rps, enemy.rps))
                {
                    laneEnemy -= _ap;
                }
                if (arena.data.rpsSetData.beats(enemy.rps, ally.rps))
                {
                    laneAlly -= _ep;
                }
                allyPower += laneAlly;
                enemyPower += laneEnemy;
            }
        }
        enemyPower = Mathf.Clamp(enemyPower, 0, enemyPower);
        allyPower = Mathf.Clamp(allyPower, 0, allyPower);
    }
}
