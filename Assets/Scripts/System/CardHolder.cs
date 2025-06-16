using System.Collections.Generic;
using System.Linq;

public class CardHolder<T> where T : Card
{
    public List<T> cardList;
    public Wrangler owner;
    public int limit = 1;

    public T Card => cardList.FirstOrDefault();
    public int CardCount => cardList.Count;

    public bool hasCard(T card)
    {
        return cardList.Contains(card);
    }


    public bool canAcceptCard(T card)
    {
        return cardList.Count < limit;
    }

    public void acceptDrop(T card)
    {
        //TODO: make CardData and Card class separate, and reinstate this
        //if (card.holder != null)
        //{
        //    card.holder.removeCard(card);
        //}
        cardList.Add(card);
        //card.holder = this;
    }


    public void removeCard(T card)
    {
        cardList.Remove(card);
    }

}
