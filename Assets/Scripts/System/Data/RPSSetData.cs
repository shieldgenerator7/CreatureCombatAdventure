using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "RPS", order = 0)]
public class RPSSetData : ScriptableObject
{
    public List<RPSLineData> lines;

    /// <summary>
    /// Returns true if rps1 beats rps2
    /// False does NOT mean that rps2 beats rps1
    /// </summary>
    /// <param name="rps1"></param>
    /// <param name="rps2"></param>
    /// <returns></returns>
    public bool beats(RockPaperScissors rps1, RockPaperScissors rps2)
    {
        return lines.Any(line => line.winner == rps1 && line.loser == rps2);
    }
}
