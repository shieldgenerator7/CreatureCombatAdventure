using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Arena",order =0)]
public class ArenaData:ScriptableObject
{
    public new string name;
    public int powerGoal = 8;
    public List<ArenaLane> lanes;
    public GameObject prefab;
}
