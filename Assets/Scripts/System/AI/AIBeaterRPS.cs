using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrains/Beater RPS")]
public class AIBeaterRPS : AIBrain
{
    public int weightFutureNone = 0;
    public int weightFutureLose = -50;
    public int weightFutureDraw = 0;
    public int weightFutureWin = 50;

    public int weightCurrentNone = 10;
    public int weightCurrentLose = 25;
    public int weightCurrentDraw = 10;
    public int weightCurrentWin = -50;


    public override Move pickMove(List<Move> moves)
    {
        List<MoveWeight> weights = moves.ConvertAll(move =>
        {
            MoveWeight weight = new MoveWeight(move);

            //future state
            switch (move.winRPS)
            {
                case WinState.WIN:
                    weight.weight += weightFutureWin;
                    break;
                case WinState.NONE:
                    weight.weight += weightFutureNone;
                    break;
                case WinState.DRAW:
                    weight.weight += weightFutureDraw;
                    break;
                case WinState.LOSE:
                    weight.weight += weightFutureLose;                    
                    break;
                default:
                    throw new System.Exception($"Unknown WinState: {move.winRPS}");
            }

            //current state
            switch (move.cardState.winRPS)
            {
                case WinState.LOSE:
                    weight.weight += weightCurrentLose;
                    break;
                case WinState.NONE:
                    weight.weight += weightCurrentNone;
                    break;
                case WinState.DRAW:
                    weight.weight += weightCurrentDraw;
                    break;
                case WinState.WIN:
                    weight.weight += weightCurrentWin;
                    break;
                default:
                    throw new System.Exception($"Unknown WinState: {move.cardState.winRPS}");
            }
            if (move.type == Move.Type.PASS)
            {
                throw new Exception($"Invalid move! {move} {move.type}");
            }
            //
            return weight;
        });
        //apply priority from move type
        weights = applyPriorityToMoveWeights(weights);
        //pick one
        return pickMoveWeights(weights);
    }
}
