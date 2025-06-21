using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Match match;
    public List<EncounterData> encounterDataList;
    public int encounterIndex = 0;
    private ArenaDisplayer arenaDisplayer;
    public Wrangler playerWrangler;

    //TODO: extract system class "Game" from this class
    public List<WranglerController> controllers;
    public AIInput aiInput;

    public TMP_Text txtPowerEnemy;
    public TMP_Text txtPowerAlly;
    public TMP_Text txtPowerGoal;

    public TMP_Text txtGameResult;

    private void Awake()
    {
        StartMatch();

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

    public void StartMatch()
    {
        playerWrangler = playerWrangler.clone();

        match = new Match();

        match.wranglers.Add(playerWrangler);
        match.encounterData = encounterDataList[encounterIndex];

        match.init();
        match.OnGameEnd += () =>
        {
            txtGameResult.gameObject.SetActive(true);
        };
        txtGameResult.gameObject.SetActive(false);

        createPlayers();
        createArena();
        updateDisplay();
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
        GameObject goArena = Instantiate(match.arena.data.prefab, Bin.Transform);
        ArenaDisplayer ad = goArena.GetComponent<ArenaDisplayer>();
        ad.arena = match.arena;
        ad.init(match.wranglers[0], match.wranglers[1]);
        arenaDisplayer = ad;
        //put cards into arena
        controllers[0].Wrangler.cardList.ForEach(card=>ad.arena.allyHand.acceptDrop(card));
        controllers[1].Wrangler.cardList.ForEach(card => ad.arena.enemyHand.acceptDrop(card));
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
        match.calculateScores();

        txtPowerEnemy.text = $"{Utility.GetSymbolString(match.enemyPower)}";
        txtPowerAlly.text = $"{Utility.GetSymbolString(match.allyPower)}";
        txtPowerGoal.text= $"{Utility.GetSymbolString(match.arena.data.powerGoal)}";

        arenaDisplayer.updateDisplay();

        txtGameResult.text = (match.winner) ? $"{match.winner.name} WINS" : "DRAW";

    }

    


    internal void Reset()
    {
        Bin.Instance.clearBin();
        StartMatch();
    }
    internal void NextMatch()
    {
        encounterIndex = Mathf.Clamp(encounterIndex+1, 0, encounterDataList.Count-1);
        Reset();
    }
}
