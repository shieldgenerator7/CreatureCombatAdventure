using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu(menuName = "AIBrains/")]
public abstract class AIBrain : ScriptableObject
{
    [Header("Move Weights")]
    public int weightMovePass = -50;
    public int weightMovePlay = 50;
    public int weightMoveMove = 25;
    public int weightMoveReorder = 10;
    //TODO: implement this
    public int weightMoveActivate = 0;

    public abstract Move pickMove(List<Move> moves);

    protected Move pickMoveRandom(List<Move> moves)
    {
        int index = UnityEngine.Random.Range(0, moves.Count);
        return moves[index];
    }

    protected Move pickMoveRandomPriority(List<Move> moves)
    {
        List<MoveWeight> weights = moves.ConvertAll(move =>
        {
            MoveWeight weight = new MoveWeight(move);
            return weight;
        });
        weights = applyPriorityToMoveWeights(weights);
        return pickMoveWeights(weights);
    }
    protected List<MoveWeight> applyPriorityToMoveWeights(List<MoveWeight> moveWeights)
    {
        return moveWeights.ConvertAll(weight =>
        {
            switch (weight.move.type)
            {//
                case Move.Type.PASS:
                    weight.weight += weightMovePass;
                    break;
                case Move.Type.PLAY:
                    weight.weight += weightMovePlay;
                    break;
                case Move.Type.MOVE:
                    weight.weight += weightMoveMove;
                    break;
                case Move.Type.REORDER:
                    weight.weight += weightMoveReorder;
                    break;
                case Move.Type.ACTIVATE:
                    weight.weight += weightMoveActivate;
                    break;
                default:
                    throw new System.Exception($"Unknown Move.Type: {weight.move.type}");
            }//
            return weight;
        });
    }

    protected Move pickMoveFilter(List<Move> moves, Predicate<Move> filterFunc)
    {
        List<Move> filteredMoves = moves.FindAll(filterFunc);
        if (filteredMoves.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, filteredMoves.Count);
            return filteredMoves[index];
        }
        return new Move(null, null);
    }

    protected Move pickMoveWeights(List<MoveWeight> moveWeights)
    {
        int max = moveWeights.Max(weight => weight.weight);
        List<Move> moves = moveWeights
            .Where(weight => weight.weight == max).ToList()
            .ConvertAll(mv => mv.move);
        return pickMoveRandom(moves);
    }
}
