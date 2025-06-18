using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIInput : WranglerInput
{

    public override void processTurn()
    {
        base.processTurn();
        //try playing cards
        if (canPlayAnyCard()) {
        //find card
        List<Card> unplayedCardList = controller.Wrangler.cardList.FindAll(card => card.holder == null || controller.Wrangler.handHolder.hasCard(card));
            if (unplayedCardList.Count == 0)
            {
                unplayedCardList = controller.Wrangler.cardList;
            }
            int randIndex1 = Random.Range(0, unplayedCardList.Count);
            Card card = unplayedCardList[randIndex1];
        if (card && controller.canPickupCard(card))
        {
            List<CardHolder> emptyHolderList = controller.Wrangler.cardHolders.FindAll((holder) => holder.CardCount == 0);
                if (emptyHolderList.Count == 0)
                {
                    emptyHolderList = controller.Wrangler.cardHolders.FindAll(holder=>holder.CardCount<holder.limit);
                }
            if (emptyHolderList.Count > 0)
            {
                int randIndex = Random.Range(0, emptyHolderList.Count);
                CardHolder ch = emptyHolderList[randIndex];
                if (controller.canPlaceCardAt(card, ch))
                {
                    controller.placeCard(card, ch);
                }
            }
        }
        }
    }

    private bool canPlayAnyCard()
    {
        return controller.Wrangler.cardList.Any(c =>
            //can be played
            controller.canPickupCard(c)
            //can be placed at any position
            && controller.Wrangler.cardHolders.Any(h => controller.canPlaceCardAt(c, h))
            );
    }
}
