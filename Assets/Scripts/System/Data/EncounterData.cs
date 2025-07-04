using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Encounter")]
public class EncounterData : ScriptableObject
{
    public new string name;
    public int powerGoal = 0;//0=use power goal of arena
    public ArenaData arena;
    public AIBrain aiBrain;
    public List<CreatureCardData> enemyCreatures;
    public List<SecretData> secrets;
}
