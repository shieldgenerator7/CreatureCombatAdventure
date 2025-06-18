using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ArenaDisplayer : MonoBehaviour
{
    public List<CardHolderDisplayer> allyHolders;
    public CardHolderDisplayer allyHand;
    public List<CardHolderDisplayer> enemyHolders;
    public CardHolderDisplayer enemyHand;

    public Arena arena;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void init(Wrangler ally, Wrangler enemy)
    {
        
        //TODO: follow DRY

        //holders ally
        ally.handHolder = new CardHolder(10);
        ally.handHolder.isHand = true;
        ally.handHolder.owner = ally;
        allyHand.init(ally.handHolder);
        ally.cardHolders = new List<CardHolder>();
        for (int i = 0; i < allyHolders.Count; i++)
        {
            CardHolder ch = new CardHolder(arena.data.lanes[i].limit);
            ch.owner = ally;
            ally.cardHolders.Add(ch);
            allyHolders[i].init(ch);
        }

        //holders enemy
        enemy.handHolder = new CardHolder(10);
        enemy.handHolder.isHand = true;
        enemy.handHolder.owner = enemy;
        enemyHand.init(enemy.handHolder);
        enemy.cardHolders = new List<CardHolder>();
        for (int i = 0; i < enemyHolders.Count; i++)
        {
            CardHolder ch = new CardHolder(arena.data.lanes[i].limit);
            ch.owner = enemy;
            enemy.cardHolders.Add(ch);
            enemyHolders[i].init(ch);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
