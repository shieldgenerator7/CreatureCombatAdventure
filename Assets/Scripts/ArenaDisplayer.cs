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
}
