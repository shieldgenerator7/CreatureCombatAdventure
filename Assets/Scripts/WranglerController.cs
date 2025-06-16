using System;
using System.Collections.Generic;
using UnityEngine;

public class WranglerController : MonoBehaviour
{
    public Wrangler player;

    public GameObject cardDisplayerPrefab;
    private List<CardDisplayer> cardDisplayerList = new List<CardDisplayer>();

    public List<CardHolderDisplayer> handHolderList;

    private void Awake()
    {
        createCards();

        for (int i = 0; i < cardDisplayerList.Count; i++)
        {
            CardDisplayer card = cardDisplayerList[i];
            card.cardLayer = i + 1;
            card.updateDisplay();
            //TODO: make function to layout cards in hand / other holder
            //add to hand holder
            int holderIndex = i % handHolderList.Count;
            handHolderList[holderIndex].acceptDrop(card);
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

    public bool canPickupCard(CardDisplayer cardDisplayer)
    {
        return cardDisplayer.card.owner == this.player;
    }

    public bool canPlaceCardAt(CardDisplayer cardDisplayer, CardHolder cardHolder)
    {
        return cardDisplayer.card.owner == this.player && cardHolder.owner == this.player
            && (cardHolder.canAcceptCard(cardDisplayer.card) || cardHolder.hasCard(cardDisplayer.card));
    }

    public void placeCard(CardDisplayer card, CardHolder holder)
    {
        if (card.card.holder)
        {
            card.card.holder.removeCard(card.card);
        }
        holder.acceptDrop(card.card);
        OnCardPlaced?.Invoke();
        OnTurnTaken?.Invoke();
    }
    public event Action OnCardPlaced;
    public event Action OnTurnTaken;
}
