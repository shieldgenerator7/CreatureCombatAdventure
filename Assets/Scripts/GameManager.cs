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

        aiInput.aiBrain = match.encounterData.aiBrain;
        createPlayers();
        createArena();

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
            updateDisplayNow(false);
        };
        txtGameResult.gameObject.SetActive(false);


        updateDisplayNow();
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
        ad.match = match;
        ad.arena = match.arena;
        ad.init(match.wranglers[0], match.wranglers[1]);
        arenaDisplayer = ad;
        //put cards into arena
        controllers[0].Wrangler.cardList.ForEach(card => ad.arena.allyHand.acceptDrop(card));
        controllers[1].Wrangler.cardList.ForEach(card => ad.arena.enemyHand.acceptDrop(card));
    }

    void updateDisplay(int pap, int ap, int pep, int ep)
    {
        List<UIAnimation> anims = new List<UIAnimation>();
        if (pep != ep)
        {
            NumberUIAnimation nuia =
        NumberUIAnimation.adjustTo(txtPowerEnemy, pep, ep, Color.white, symbolSetData);
            anims.Add(nuia);
        }
        if (pap != ap)
        {
            NumberUIAnimation nuia =
        NumberUIAnimation.adjustTo(txtPowerAlly, pap, ap, Color.white, symbolSetData);
            anims.Add(nuia);
        }
        if (anims.Count > 1)
        {
            SimultaneousUIAnimation.AnimateTogether(anims);
        }
    }

    void updateDisplayNow(bool updateWranglerPower = true)
    {
        if (updateWranglerPower) {
        txtPowerAlly.text = $"{symbolSetData.GetSymbolString(match.allyPower)}";
        txtPowerEnemy.text = $"{symbolSetData.GetSymbolString(match.enemyPower)}";
        }
        txtPowerGoal.text = $"{symbolSetData.GetSymbolString(match.powerGoal)}";

        txtGameResult.text = (match.winner) ? $"{match.winner.name} WINS" : "DRAW";
    }




    internal void Reset()
    {
        match.close();
        Bin.Instance.clearBin();
        UIAnimationQueue.Instance.Reset();
        StartMatch();
        updateDisplayNow();
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
