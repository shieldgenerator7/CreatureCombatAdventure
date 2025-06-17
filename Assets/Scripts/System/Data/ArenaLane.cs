using System;
using UnityEngine;

[Serializable]
public struct ArenaLane
{
    [Tooltip("How many cards can be in this lane (max)")]
    public int limit;
}
