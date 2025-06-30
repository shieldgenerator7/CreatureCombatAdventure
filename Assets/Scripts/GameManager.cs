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
    public SymbolSetData symbolSetData;
    public static SymbolSetData SymbolSetData => FindAnyObjectByType<GameManager>().symbolSetData;

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
    }

    public void StartMatch()
    {
        playerWrangler = playerWrangler.clone();

        match = new Match();

        match.wranglers.Add(playerWrangler);
        match.encounterData = encounterDataList[encounterIndex];

        match.init();

        match.OnScoresChanged += updateDisplay;
        match.OnGameEnd += () =>
        {
            ShowUIAnimation.showObject(txtGameResult.gameObject, true);
            if (match.winner == playerWrangler)
            {
                int rand = UnityEngine.Random.Range(0, match.wranglers[1].cardList.Count);
                CreatureCardData cardData = match.wranglers[1].cardList[rand].data;
                playerWrangler.addCard(cardData);
            }
            updateDisplay();
        };
        txtGameResult.gameObject.SetActive(false);

        aiInput.aiBrain = match.encounterData.aiBrain;

        createPlayers();
        createArena();
        updateDisplay();
    }

    private void createPlayers()
    {
        for (int i = 0; i < match.wranglers.Count; i++)
        {
            controllers[i].init(match.wranglers[i]);
        }

        aiInput.controller.Wrangler.OnTurnStarted += aiInput.processTurn;
    }

    private void createArena()
    {
        GameObject goArena = Instantiate(match.arena.data.prefab, Bin.Transform);
        ArenaDisplayer ad = goArena.GetComponent<ArenaDisplayer>();
        ad.arena = match.arena;
        ad.init(match.wranglers[0], match.wranglers[1]);
        arenaDisplayer = ad;
        //put cards into arena
        controllers[0].Wrangler.cardList.ForEach(card => ad.arena.allyHand.acceptDrop(card));
        controllers[1].Wrangler.cardList.ForEach(card => ad.arena.enemyHand.acceptDrop(card));
    }//

    // Update is called once per frame
    void Update()
    {

    }


    void updateDisplay(int pap, int ap, int pep, int ep)
    {
        if (pep != ep)
        {
        NumberUIAnimation.adjustTo(txtPowerEnemy, pep, ep, symbolSetData);
        }
        if (pap != ap)
        {
        NumberUIAnimation.adjustTo(txtPowerAlly, pap, ap, symbolSetData);
        }

    }

    void updateDisplay()
    {
        txtPowerGoal.text = $"{symbolSetData.GetSymbolString(match.powerGoal)}";

        txtGameResult.text = (match.winner) ? $"{match.winner.name} WINS" : "DRAW";
    }




    internal void Reset()
    {
        match.close();
        Bin.Instance.clearBin();
        UIAnimationQueue.Instance.Reset();
        StartMatch();
    }
    internal void NextMatch()
    {
        match.close();
        encounterIndex = Mathf.Clamp(encounterIndex + 1, 0, encounterDataList.Count - 1);
        Reset();
    }
    internal void PrevMatch()
    {
        match.close();
        encounterIndex = Mathf.Clamp(encounterIndex - 1, 0, encounterDataList.Count - 1);
        Reset();
    }

    internal bool GameOver => match.winner != null;
}
