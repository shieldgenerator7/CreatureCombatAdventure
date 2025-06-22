using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AIBrains/Random")]
public class AIRandom : AIBrain
{
    public override Move pickMove(List<Move> moves)
    {
        int index = Random.Range(0, moves.Count);
        return moves[index];
    }
}
