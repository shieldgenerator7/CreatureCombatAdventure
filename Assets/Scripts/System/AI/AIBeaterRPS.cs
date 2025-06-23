using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrains/Beater RPS")]
public class AIBeaterRPS : AIBrain
{
    public override Move pickMove(List<Move> moves)
    {
        Move filteredMove;
        //Try 1: Move that beats RPS
        filteredMove = pickMoveFilter(moves, move => move.winRPS == WinState.WIN);
        if (filteredMove.Valid) { return filteredMove; }
        //Try 2: Move a card that is currently losing RPS
        filteredMove = pickMoveFilter(moves, move => move.cardState.winRPS == WinState.LOSE);
        if (filteredMove.Valid) { return filteredMove; }
        //Try 3: Move a card that isn't currently beating RPS
        filteredMove = pickMoveFilter(moves, move => move.cardState.winRPS == WinState.NONE || move.cardState.winRPS == WinState.DRAW);
        if (filteredMove.Valid) { return filteredMove; }
        //Default: random move
        return pickMoveRandomPriority(moves);
    }
}
