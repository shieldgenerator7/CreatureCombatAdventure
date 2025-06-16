using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIInput : WranglerInput
{

    public List<CardHolderDisplayer> holders;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void processTurn()
    {
        base.processTurn();
        //try playing cards
        if (canPlayAnyCard()) {
        //find card
        List<Card> unplayedCardList = controller.player.cardList.FindAll(card => card.holder == null || controller.player.handHolder.hasCard(card));
            int randIndex1 = Random.Range(0, unplayedCardList.Count);
            Card card = unplayedCardList[randIndex1];
        if (card && controller.canPickupCard(card))
        {
            List<CardHolder> emptyHolderList = controller.player.cardHolders.FindAll((holder) => holder.CardCount == 0);
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
        return controller.player.cardList.Any(c =>
            //not on the board
            (c.holder == null || controller.player.handHolder.hasCard(c))
            //can be played
            && controller.canPickupCard(c)
            //can be placed at any position
            && controller.player.cardHolders.Any(h => controller.canPlaceCardAt(c, h))
            );
    }
}
