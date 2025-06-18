using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Match match;
    private ArenaDisplayer arenaDisplayer;

    //TODO: extract system class "Game" from this class
    public List<WranglerController> controllers;
    public AIInput aiInput;

    public TMP_Text txtPowerEnemy;
    public TMP_Text txtPowerAlly;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        match.init();

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
        //updateDisplay();
        createPlayers();
        createArena();
    }

    private void createPlayers()
    {
        for(int i = 0; i < match.wranglers.Count; i++)
        {
            controllers[i].init(match.wranglers[i]);
        }
    }

    private void createArena()
    {
        GameObject goArena = Instantiate(match.arena.data.prefab);
        ArenaDisplayer ad = goArena.GetComponent<ArenaDisplayer>();
        ad.arena = match.arena;
        ad.init(match.wranglers[0], match.wranglers[1]);
        arenaDisplayer = ad;
        //put cards into arena
        controllers[0].CardDisplayerList.ForEach(cd=>ad.arena.allyHand.acceptDrop(cd.card));
        controllers[1].CardDisplayerList.ForEach(cd => ad.arena.enemyHand.acceptDrop(cd.card));
    }//

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
            int laneAlly = arenaDisplayer.arena.allyHolders[i].cardList.Sum(card => card.data.power);
            int laneEnemy = arenaDisplayer.arena.enemyHolders[i].cardList.Sum(card => card.data.power);
            CreatureCardData ally = arenaDisplayer.arena.allyHolders[i].Card?.data;
            CreatureCardData enemy = arenaDisplayer.arena.enemyHolders[i].Card?.data;
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
                int _ap = ally.power;
                int _ep = enemy.power;
                if (beats(ally.rps, enemy.rps))
                {
                    laneEnemy -= _ap;
                }
                if (beats(enemy.rps, ally.rps))
                {
                    laneAlly -= _ep;
                }
                allyPower += laneAlly;
                enemyPower += laneEnemy;
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
