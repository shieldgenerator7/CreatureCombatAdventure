using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Match
{
    public EncounterData encounterData;
    public List<Wrangler> wranglers = new List<Wrangler>();
    [NonSerialized]
    public Arena arena;
    [NonSerialized]
    public int enemyPower = 0;
    [NonSerialized]
    public int allyPower = 0;

    [NonSerialized]
    public Wrangler winner = null;

    public void init()
    {
        arena = new Arena();
        arena.data = encounterData.arena;
        arena.init();

        //players (ally)
        //do nothing (assume setup in scene)

        //players (enemy)
        Wrangler enemy;
        if (wranglers.Count < 2)
        {
            enemy = new Wrangler();
            enemy.name = encounterData.name;
            enemy.cardList.AddRange(encounterData.enemyCreatures.ConvertAll(data=>new Card(data)));
            wranglers.Add(enemy);
        }

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
        enemy = wranglers[1];
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

        if (!winner)
        {
            Wrangler ally = wranglers[0];
            Wrangler enemy = wranglers[1];
            int powerGoal = (encounterData.powerGoal > 0)?encounterData.powerGoal : arena.data.powerGoal;
            bool allyGoal = allyPower >= powerGoal;
            bool enemyGoal = enemyPower >= powerGoal;
            if (allyGoal || enemyGoal)
            {
                if (allyGoal && enemyGoal)
                {
                    if (allyPower > enemyPower)
                    {
                        winner = ally;
                    }
                    else if (enemyPower > allyPower)
                    {
                        winner = enemy;
                    }
                    else
                    {
                        winner = null;
                    }
                }
                else if (allyGoal)
                {
                    winner = ally;
                }
                else if (enemyGoal)
                {
                    winner = enemy;
                }
                OnGameEnd?.Invoke();
            }
        }
    }
    public event Action OnGameEnd;
}
