using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArenaDisplayer : MonoBehaviour
{
    public List<CardHolderDisplayer> allyHolders;
    public CardHolderDisplayer allyHand;
    public List<CardHolderDisplayer> enemyHolders;
    public CardHolderDisplayer enemyHand;

    public List<TMP_Text> txtAllyList;
    public List<TMP_Text> txtEnemyList;

    public Arena arena;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void init(Wrangler ally, Wrangler enemy)
    {
        
        //TODO: follow DRY

        //holders ally
        allyHand.init(ally.handHolder);
        for (int i = 0; i < arena.allyHolders.Count; i++)
        {
            CardHolder ch = arena.allyHolders[i];
            allyHolders[i].init(ch);
        }

        //holders enemy
        enemyHand.init(enemy.handHolder);
        for (int i = 0; i < arena.enemyHolders.Count; i++)
        {
            CardHolder ch = arena.enemyHolders[i];
            enemyHolders[i].init(ch);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateDisplay()
    {
        for (int i = 0; i < arena.lanes.Count; i++)
        {
            ArenaLane lane = arena.lanes[i];
            txtAllyList[i].text = $"{Utility.GetSymbolString(lane.AllyRPS)}  {Utility.GetSymbolString(lane.allyPower)}";
            txtEnemyList[i].text = $"{Utility.GetSymbolString(lane.EnemyRPS)}  {Utility.GetSymbolString(lane.enemyPower)}";
        }
    }
}
