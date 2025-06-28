using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AIBrains/Random")]
public class AIRandom : AIBrain
{

    public override Move pickMove(List<Move> moves)
    {
        return pickMoveRandomPriority(moves);
    }
}
