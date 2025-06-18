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
        arena.init();

        //holders ally
        ally.handHolder = arena.allyHand;
        ally.handHolder.owner = ally;
        allyHand.init(ally.handHolder);
        ally.cardHolders = arena.allyHolders;
        for (int i = 0; i < arena.allyHolders.Count; i++)
        {
            CardHolder ch = arena.allyHolders[i];
            ch.owner = ally;
            allyHolders[i].init(ch);
        }

        //holders enemy
        enemy.handHolder = arena.enemyHand;
        enemy.handHolder.owner = enemy;
        enemyHand.init(enemy.handHolder);
        enemy.cardHolders = arena.enemyHolders;
        for (int i = 0; i < arena.enemyHolders.Count; i++)
        {
            CardHolder ch = arena.enemyHolders[i];
            ch.owner = enemy;
            enemyHolders[i].init(ch);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
