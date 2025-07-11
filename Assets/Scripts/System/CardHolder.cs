using System;
using System.Collections.Generic;
using System.Linq;

public class CardHolder
{
    public List<Card> cardList = new List<Card>();
    public Wrangler owner;
    public int limit = 1;
    public bool isHand = false;
    [NonSerialized]
    public CardHolder opposingHolder;
    [NonSerialized]
    public Arena arena;

    public Card Card => cardList.FirstOrDefault();
    public int CardCount => cardList.Count;

    public CardHolder(int limit = 1)
    {
        this.limit = limit;
    }

    public bool hasCard(Card card)
    {
        return cardList.Contains(card);
    }


    public bool canAcceptCard(Card card)
    {
        return cardList.Count < limit;
    }

    public void acceptDrop(Card card)
    {
        if (card.holder != null)
        {
            card.holder.removeCard(card);
        }
        cardList.Add(card);
        card.holder = this;
        OnCardDropped?.Invoke(card);
    }
    public event Action<Card> OnCardDropped;


    public void removeCard(Card card)
    {
        cardList.Remove(card);
        OnCardRemoved?.Invoke(card);
    }
    public event Action<Card> OnCardRemoved;

    public int PowerRaw => cardList.Sum(card => card.data.power);


    public static implicit operator bool(CardHolder cardHolder) => cardHolder != null;

}
