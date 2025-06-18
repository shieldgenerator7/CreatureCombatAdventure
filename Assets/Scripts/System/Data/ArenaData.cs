using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Arena",order =0)]
public class ArenaData:ScriptableObject
{
    public new string name;
    [Tooltip("When a player reaches this power goal, they win the match")]
    public int powerGoal = 8;
    [Tooltip("The max number of cards a player can have in their hand")]
    public int handSize = 10;
    public List<ArenaLane> lanes;
    public RPSSetData rpsSetData;
    public GameObject prefab;
}
