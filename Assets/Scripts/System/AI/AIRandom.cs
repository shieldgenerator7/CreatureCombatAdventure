using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="AIBrains/Random")]
public class AIRandom : AIBrain
{
    public bool prioritizePlaying = true;

    public override Move pickMove(List<Move> moves)
    {
        if (prioritizePlaying)
        {
            List<Move> playMoves = moves.FindAll(move => move.isMovePlay);
            if (playMoves.Count > 0)
            {
                int playindex = Random.Range(0, playMoves.Count);
                return playMoves[playindex];
            }
        }
        int index = Random.Range(0, moves.Count);
        return moves[index];
    }
}
