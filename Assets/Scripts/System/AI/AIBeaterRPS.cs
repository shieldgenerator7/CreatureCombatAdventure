using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrains/Beater RPS")]
public class AIBeaterRPS : AIBrain
{
    public override Move pickMove(List<Move> moves)
    {
        Move filteredMove = pickMoveFilter(moves, move => move.beatsOpposingRPS);
        return (filteredMove.Valid)
            ? filteredMove
            : pickMoveRandomPriority(moves);
    }
}
