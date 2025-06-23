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

            //future state
            switch (move.winRPS)
            {
                //Priority 1: Move that beats RPS
                case WinState.WIN:
                    weight.weight += 50;
                    break;
                //don't weight these
                case WinState.NONE:
                case WinState.DRAW:
                    break;
                //dont go somewhere where you lose
                case WinState.LOSE:
                    weight.weight -= 50;
                    break;
                default:
                    throw new System.Exception($"Unknown WinState: {move.winRPS}");
            }

            //current state
            switch (move.cardState.winRPS)
            {
                //Priority 2: Move a card that is currently losing RPS
                case WinState.LOSE:
                    weight.weight += 25;
                    break;

                //Priority 3: Move a card that isn't currently beating RPS
                case WinState.NONE:
                case WinState.DRAW:
                    weight.weight += 10;
                    break;
                //dont move if youre already winning
                case WinState.WIN:
                    weight.weight -= 50;
                    break;
                default:
                    throw new System.Exception($"Unknown WinState: {move.cardState.winRPS}");
            }
            //
            return weight;
        });
        return pickMoveWeights(weights);
    }
}
