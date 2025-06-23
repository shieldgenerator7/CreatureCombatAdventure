using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Match
{
    public EncounterData encounterData;
    public List<Wrangler> wranglers = new List<Wrangler>();
    public int powerGoal = 0;
    [NonSerialized]
    public Arena arena;
    [NonSerialized]
    public int enemyPower = 0;
    [NonSerialized]
    public int allyPower = 0;

    [NonSerialized]
    private Wrangler currentWrangler = null;
    [NonSerialized]
    public Wrangler winner = null;

    public void init()
    {
        arena = new Arena();
        arena.data = encounterData.arena;
        arena.init();

        //players (ally)
        //(assume setup in scene)
        Wrangler ally = wranglers[0];
        ally.init(arena.allyHolders);
        ally.OnTurnEnded += processNextTurn;

        //players (enemy)
        Wrangler enemy;
        if (wranglers.Count < 2)
        {
            enemy = new Wrangler();
            enemy.name = encounterData.name;
            enemy.cardList.AddRange(encounterData.enemyCreatures.ConvertAll(data=>new Card(data)));
            enemy.init(arena.enemyHolders);
            enemy.OnTurnEnded += processNextTurn;
            wranglers.Add(enemy);
        }

        //holders ally
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

        //power goal
        powerGoal = (encounterData.powerGoal > 0) ? encounterData.powerGoal : arena.data.powerGoal;

        //start
        currentWrangler = ally;
        currentWrangler.startTurn();
    }

    public void processNextTurn()
    {
        Wrangler prevWrangler = currentWrangler;
        int index = wranglers.IndexOf(prevWrangler);
        index++;
        if (index >= wranglers.Count)
        {
            index = 0;
        }
        currentWrangler = wranglers[index];
        currentWrangler.startTurn();
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

        calculatePlayerWin(wranglers[0]);
        calculatePlayerWin(wranglers[1]);
    }
    public void calculatePlayerWin(Wrangler wrangler)
    {
        if (!winner)
        {
            bool goal = ((wrangler == wranglers[0])?allyPower:enemyPower) >= powerGoal;
            if (goal)
            {
                winner = wrangler;
                OnGameEnd?.Invoke();
            }
        }
    }
    public event Action OnGameEnd;
}
