using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName = "AIBrains/")]
public abstract class AIBrain:ScriptableObject
{
    public abstract Move pickMove(List<Move> moves);
}
