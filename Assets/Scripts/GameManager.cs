using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ArenaData arenaData;
    private ArenaDisplayer arenaDisplayer;
    private Wrangler ally;
    private Wrangler enemy;

    //TODO: extract system class "Game" from this class
    public List<WranglerController> controllers;
    public AIInput aiInput;

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
        //updateDisplay();
        createPlayers();
        createArena();
    }

    private void createPlayers()
    {
        ally = controllers[0].player;
        enemy = controllers[1].player;
        controllers.ForEach(c => c.createCards());
    }

    private void createArena()
    {
        GameObject goArena = Instantiate(arenaData.prefab);
        ArenaDisplayer ad = goArena.GetComponent<ArenaDisplayer>();
        ad.init(ally, enemy);
        arenaDisplayer = ad;
        //put cards into arena
        controllers[0].CardDisplayerList.ForEach(cd=>ad.allyHand.CardHolder.acceptDrop(cd.card));
        controllers[1].CardDisplayerList.ForEach(cd => ad.enemyHand.CardHolder.acceptDrop(cd.card));
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
            int laneAlly = arenaDisplayer.allyHolders[i].CardHolder.cardList.Sum(card => card.data.power);
            int laneEnemy = arenaDisplayer.enemyHolders[i].CardHolder.cardList.Sum(card => card.data.power);
            CreatureCardData ally = arenaDisplayer.allyHolders[i].CardHolder.Card?.data;
            CreatureCardData enemy = arenaDisplayer.enemyHolders[i].CardHolder.Card?.data;
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
