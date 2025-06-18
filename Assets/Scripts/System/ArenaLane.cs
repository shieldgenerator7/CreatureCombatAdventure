using System.Linq;
using UnityEngine;

public class ArenaLane
{
    public ArenaLaneData data;
    public RPSSetData rpsData;

    public CardHolder allyHolder;
    public CardHolder enemyHolder;

    public int allyPower;
    public int enemyPower;

    public RockPaperScissors AllyRPS => allyHolder.Card?.data.rps ?? RockPaperScissors.NONE;
    public RockPaperScissors EnemyRPS => enemyHolder.Card?.data.rps ?? RockPaperScissors.NONE;

    public int AllyPowerRaw => allyHolder.cardList.Sum(card => card.data.power);
    public int EnemyPowerRaw => enemyHolder.cardList.Sum(card => card.data.power);

    public void calculatePower()
    {
        allyPower = 0;
        enemyPower = 0;

        int laneAlly = AllyPowerRaw;
        int laneEnemy = EnemyPowerRaw;
        CreatureCardData ally = allyHolder.Card?.data;
        CreatureCardData enemy = enemyHolder.Card?.data;
        if (!enemy)
        {
            allyPower += laneAlly;
        }
        if (!ally)
        {
            enemyPower += laneEnemy;
        }
        if (ally && enemy)
        {
            if (rpsData.beats(ally.rps, enemy.rps))
            {
                laneEnemy -= ally.power;
            }
            if (rpsData.beats(enemy.rps, ally.rps))
            {
                laneAlly -= enemy.power;
            }
            allyPower += laneAlly;
            enemyPower += laneEnemy;
        }

        enemyPower = Mathf.Clamp(enemyPower, 0, enemyPower);
        allyPower = Mathf.Clamp(allyPower, 0, allyPower);
    }
}
