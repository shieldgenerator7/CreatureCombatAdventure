using System;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "AIBrains/")]
public abstract class AIBrain:ScriptableObject
{
    [Tooltip("Which move it should prioritize. Set to PASS to not prioritize anything")]
    public Move.Type priority;

    public abstract Move pickMove(List<Move> moves);

    protected Move pickMoveRandom(List<Move> moves)
    {
        int index = UnityEngine.Random.Range(0, moves.Count);
        return moves[index];
    }

    protected Move pickMoveRandomPriority(List<Move> moves)
    {
        if (priority != Move.Type.PASS)
        {
            Move priorityMove = pickMoveFilter(moves, move => move.type == priority);
            if (priorityMove.Valid)
            {
                return priorityMove;
            }
        }
        return pickMoveRandom(moves);
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
