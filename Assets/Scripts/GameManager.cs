using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<WranglerController> controllers;
    public AIInput aiInput;


    public List<CardHolder> holders;

    public List<CardHolder> enemyRanks;
    public List<CardHolder> allyRanks;

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
            CreatureCard ally = allyRanks[i].card;
            CreatureCard enemy = enemyRanks[i].card;
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

        txtPowerEnemy.text = $"{enemyPower}";
        txtPowerAlly.text = $"{allyPower}";

    }

    //TODO: move to more suitable spot (utility?)
    public bool beats(RockPaperScissors rps1, RockPaperScissors rps2)
    {
        return rps1 == RockPaperScissors.ROCK && rps2 == RockPaperScissors.SCISSORS
            || rps1 == RockPaperScissors.PAPER && rps2 == RockPaperScissors.ROCK
            || rps1 == RockPaperScissors.SCISSORS && rps2 == RockPaperScissors.PAPER;
    }


}
