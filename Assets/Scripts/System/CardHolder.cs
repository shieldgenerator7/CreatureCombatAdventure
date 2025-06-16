using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class CardHolder
{
    [NonSerialized]
    public List<Card> cardList = new List<Card>();
    [NonSerialized]
    public Wrangler owner;
    public int limit = 1;

    public Card Card => cardList.FirstOrDefault();
    public int CardCount => cardList.Count;

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
    }


    public static implicit operator bool(CardHolder cardHolder) => cardHolder != null;

}
