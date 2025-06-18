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

    public List<ArenaLane> lanes;

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
        lanes = new List<ArenaLane>();
        for (int i = 0; i < data.lanes.Count; i++)
        {
            ArenaLaneData ald = data.lanes[i];
            int limit = ald.limit;
            CardHolder allyHolder = new CardHolder(limit);
            allyHolders.Add(allyHolder);
            CardHolder enemyHolder = new CardHolder(limit);
            enemyHolders.Add(enemyHolder);

            //lanes
            ArenaLane lane = new ArenaLane();
            lane.data = ald;
            lane.rpsData = data.rpsSetData;
            lane.allyHolder = allyHolder;
            lane.enemyHolder = enemyHolder;
            lanes.Add(lane);

        }
    }
}
