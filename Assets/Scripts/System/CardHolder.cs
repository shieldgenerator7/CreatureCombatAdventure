using System.Collections.Generic;
using System.Linq;

public class CardHolder
{
    public List<CreatureCard> cardList;
    public Wrangler owner;
    public int limit = 1;

    public CreatureCard Card => cardList.FirstOrDefault();
    public int CardCount => cardList.Count;

    public bool hasCard(CreatureCard card)
    {
        return cardList.Contains(card);
    }


    public bool canAcceptCard(CreatureCard card)
    {
        return cardList.Count < limit;
    }

    public void acceptDrop(CreatureCard card)
    {
        //TODO: make CardData and Card class separate, and reinstate this
        //if (card.holder != null)
        //{
        //    card.holder.removeCard(card);
        //}
        cardList.Add(card);
        //card.holder = this;
    }


    public void removeCard(CreatureCard card)
    {
        cardList.Remove(card);
    }

}
