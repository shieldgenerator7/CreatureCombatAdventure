using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Wrangler
{
    public string name;
    public List<Card> cardList = new List<Card>();
    [NonSerialized]
    public List<CardHolder> cardHolders = new List<CardHolder>();
    [NonSerialized]
    public CardHolder handHolder;

    public void init(List<CardHolder> cardHolders)
    {
        this.cardHolders = cardHolders;
    }

    public void startTurn()
    {
        OnTurnStarted?.Invoke();
    }
    public event Action OnTurnStarted;

    public void endTurn()
    {
        OnTurnEnded?.Invoke();
    }
    public event Action OnTurnEnded;




    public void addCard(CreatureCardData cardData)
    {
        Card card = new Card(cardData);
        cardList.Add(card);
        handHolder.acceptDrop(card);
        OnCardAdded?.Invoke(card);
    }
    public event Action<Card> OnCardAdded;

    public bool canPickupCard(Card card)
    {
        return card.owner == this;
    }

    public bool canPlaceCardAt(Card card, CardHolder cardHolder)
    {
        return card.owner == this && cardHolder.owner == this
            && (cardHolder.canAcceptCard(card) || cardHolder.hasCard(card));
    }

    public void placeCard(Card card, CardHolder holder)
    {
        //a move is a fake move if it moves to the same holder and is either a hand, or the card was already the last in the list
        bool fakeMove = card.holder == holder && (holder.isHand || card.holder.cardList.Last() == card);
        if (card.holder)
        {
            card.holder.removeCard(card);
        }
        holder.acceptDrop(card);
        OnCardPlaced?.Invoke();
        if (!fakeMove)
        {
            endTurn();
        }
    }
    public event Action OnCardPlaced;

    public void skipTurn()
    {
        endTurn();
    }





    public Wrangler clone()
    {
        Wrangler clone = new Wrangler();
        clone.name = name;
        clone.cardList = cardList.ConvertAll(card => new Card(card.data));
        return clone;
    }

    public static implicit operator bool(Wrangler wrangler) => wrangler != null;

}
