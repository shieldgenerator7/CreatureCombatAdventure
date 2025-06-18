using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Arena
{
    public ArenaData data;
    public CardHolder allyHand;
    public List<CardHolder> allyHolders;
    public CardHolder enemyHand;
    public List<CardHolder> enemyHolders;

    public void init()
    {
        //ally
        allyHand= new CardHolder(data.handSize);
        allyHand.isHand = true;
        allyHolders = new List<CardHolder>();
        //enemy
        enemyHand = new CardHolder(data.handSize);
        enemyHand.isHand = true;
        enemyHolders = new List<CardHolder>();
        //populate lanes
        for (int i = 0; i < data.lanes.Count; i++)
        {
            int limit = data.lanes[i].limit;
            allyHolders.Add(new CardHolder(limit));
            enemyHolders.Add(new CardHolder(limit));
        }
    }
}
