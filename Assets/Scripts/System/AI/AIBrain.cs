using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "AIBrains/")]
public abstract class AIBrain:ScriptableObject
{
    public abstract Move pickMove(List<Move> moves);

    protected Move pickMoveRandom(List<Move> moves)
    {
        int index = UnityEngine.Random.Range(0, moves.Count);
        return moves[index];
    }

    protected Move pickMoveFilter(List<Move> moves, Predicate<Move> filterFunc)
    {
        List<Move> filteredMoves = moves.FindAll(filterFunc);
        if (filteredMoves.Count > 0)
        {
            int index= UnityEngine.Random.Range(0, filteredMoves.Count);
            return filteredMoves[index];
        }
        return new Move(null, null);
    }
}
