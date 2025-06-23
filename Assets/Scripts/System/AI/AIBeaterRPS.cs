using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrains/Beater RPS")]
public class AIBeaterRPS : AIBrain
{
    public override Move pickMove(List<Move> moves)
    {
        List<MoveWeight> weights = moves.ConvertAll(move =>
        {
            MoveWeight weight = new MoveWeight(move);
            //Priority 1: Move that beats RPS
            if (move.winRPS == WinState.WIN)
            {
                weight.weight += 50;
            }
            //Priority 2: Move a card that is currently losing RPS
            if (move.cardState.winRPS == WinState.LOSE)
            {
                weight.weight += 25;
            }
            //Priority 3: Move a card that isn't currently beating RPS
            if (move.cardState.winRPS == WinState.NONE || move.cardState.winRPS == WinState.DRAW)
            {
                weight.weight += 10;
            }
            return weight;
        });
        return pickMoveWeights(weights);
    }
}
