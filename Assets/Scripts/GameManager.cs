using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO: extract system class "Game" from this class
    public List<WranglerController> controllers;
    public AIInput aiInput;

    public List<CardHolderDisplayer> enemyRanks;
    public List<CardHolderDisplayer> allyRanks;

    public TMP_Text txtPowerEnemy;
    public TMP_Text txtPowerAlly;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controllers.ForEach((controller) =>
        {
            controller.OnCardPlaced += updateDisplay;
        });
        controllers
            .FindAll(c => aiInput.controller != c)
            .ForEach(c =>
                {
                    c.OnTurnTaken += processNextTurn;
                });
        updateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void processNextTurn()
    {
        aiInput.processTurn();
    }



    void updateDisplay()
    {
        int enemyPower = 0;
        int allyPower = 0;

        for (int i = 0; i < 5; i++)
        {
            CreatureCardData ally = allyRanks[i].CardHolder.Card?.data;
            CreatureCardData enemy = enemyRanks[i].CardHolder.Card?.data;
            if (!enemy)
            {
                allyPower += ally?.power ?? 0;
            }
            if (!ally)
            {
                enemyPower += enemy?.power ?? 0;
            }
            if (ally && enemy)
            {
                int _ap = ally.power;
                int _ep = enemy.power;
                if (beats(ally.rps, enemy.rps))
                {
                    _ep -= _ap;
                }
                if (beats(enemy.rps, ally.rps))
                {
                    _ap -= _ep;
                }
                allyPower += _ap;
                enemyPower += _ep;
            }
        }
        enemyPower=Mathf.Clamp(enemyPower, 0, enemyPower);
        allyPower=Mathf.Clamp(allyPower, 0, allyPower);

        txtPowerEnemy.text = $"{Utility.GetSymbolString(enemyPower)}";
        txtPowerAlly.text = $"{Utility.GetSymbolString(allyPower)}";

    }

    //TODO: move to more suitable spot (utility?)
    public bool beats(RockPaperScissors rps1, RockPaperScissors rps2)
    {
        return rps1 == RockPaperScissors.ROCK && rps2 == RockPaperScissors.SCISSORS
            || rps1 == RockPaperScissors.PAPER && rps2 == RockPaperScissors.ROCK
            || rps1 == RockPaperScissors.SCISSORS && rps2 == RockPaperScissors.PAPER;
    }


}
