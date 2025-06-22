using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AIBrains/Random")]
public class AIRandom : AIBrain
{
    [Tooltip("Which move it should prioritize. Set to PASS to not prioritize anything")]
    public Move.Type priority;

    public override Move pickMove(List<Move> moves)
    {
        if (priority != Move.Type.PASS)
        {
            List<Move> priorityMoves = moves.FindAll(move => move.type == this.priority);
            if (priorityMoves.Count > 0)
            {
                int priorityIndex = Random.Range(0, priorityMoves.Count);
                return priorityMoves[priorityIndex];
            }
        }
        int index = Random.Range(0, moves.Count);
        return moves[index];
    }
}
