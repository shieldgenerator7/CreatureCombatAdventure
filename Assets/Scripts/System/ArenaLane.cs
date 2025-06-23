using System.Linq;
using UnityEngine;

public class ArenaLane
{
    public ArenaLaneData data;
    public RPSSetData rpsData;
    public int laneId;

    public CardHolder allyHolder;
    public CardHolder enemyHolder;

    public int allyPower;
    public int enemyPower;

    public RockPaperScissors AllyRPS => allyHolder.Card?.data.rps ?? RockPaperScissors.NONE;
    public RockPaperScissors EnemyRPS => enemyHolder.Card?.data.rps ?? RockPaperScissors.NONE;

    public int AllyPowerRaw => allyHolder.PowerRaw;
    public int EnemyPowerRaw => enemyHolder.PowerRaw;

    public bool calculatePower()
    {
        int prevEnemyPower = enemyPower;
        int prevAllyPower = allyPower;

        allyPower = AllyPowerRaw;
        enemyPower = EnemyPowerRaw;
        CreatureCardData ally = allyHolder.Card?.data;
        CreatureCardData enemy = enemyHolder.Card?.data;
        if (ally && enemy)
        {
            if (rpsData.beats(ally.rps, enemy.rps))
            {
                enemyPower -= ally.power;
                enemyPower = Mathf.Clamp(enemyPower, 0, enemyPower);
            }
            if (rpsData.beats(enemy.rps, ally.rps))
            {
                allyPower -= enemy.power;
                allyPower = Mathf.Clamp(allyPower, 0, allyPower);
            }
        }


        return enemyPower != prevEnemyPower || allyPower != prevAllyPower;
    }
}
