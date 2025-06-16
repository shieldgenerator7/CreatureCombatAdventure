using System;
using System.Collections.Generic;
using UnityEngine;

public class WranglerController : MonoBehaviour
{
    public Wrangler player;

    public GameObject cardDisplayerPrefab;
    private List<CardDisplayer> cardDisplayerList = new List<CardDisplayer>();

    public CardHolderDisplayer handHolder;
    public List<CardHolderDisplayer> holderList;

    private void Awake()
    {
        //holders
        handHolder.init(player.handHolder);
        for(int i =0; i < holderList.Count; i++)
        {
            holderList[i].init(player.cardHolders[i]);
            player.cardHolders[i].owner = player;
        }

        //cards
        createCards();

        //place cards in hand holder
        for (int i = 0; i < cardDisplayerList.Count; i++)
        {
            CardDisplayer card = cardDisplayerList[i];
            card.cardLayer = i + 1;
            card.updateDisplay();
            //TODO: make function to layout cards in hand / other holder
            //add to hand holder
            handHolder.acceptDrop(card);
            //
        }
    }

    private void createCards()
    {
        cardDisplayerList.ForEach(card =>
        {
            Destroy(card.gameObject);
        });
        cardDisplayerList.Clear();
        player.cardList.ForEach(card =>
        {
            GameObject go = Instantiate(cardDisplayerPrefab);
            CardDisplayer cd = go.GetComponent<CardDisplayer>();
            cd.card = card;
            cd.updateDisplay();
            cd.transform.position = new Vector2(0, -5);
            card.owner = this.player;

            cardDisplayerList.Add(cd);
        });
    }

    public List<CardDisplayer> CardDisplayerList =>cardDisplayerList;

    public bool canPickupCard(Card card)
    {
        return card.owner == this.player;
    }

    public bool canPlaceCardAt(Card card, CardHolder cardHolder)
    {
        return card.owner == this.player && cardHolder.owner == this.player
            && (cardHolder.canAcceptCard(card) || cardHolder.hasCard(card));
    }

    public void placeCard(Card card, CardHolder holder)
    {
        if (card.holder)
        {
            card.holder.removeCard(card);
        }
        holder.acceptDrop(card);
        OnCardPlaced?.Invoke();
        OnTurnTaken?.Invoke();
    }
    public event Action OnCardPlaced;
    public event Action OnTurnTaken;
}
