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
            Move priorityMove = pickMoveFilter(moves,move=>move.type == priority);
            if (priorityMove.Valid)
            {
                return priorityMove;
            }
        }
        return pickMoveRandom(moves);
    }
}
