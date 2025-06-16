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
        List<CardDisplayer> unplayedCardList = controller.CardDisplayerList.FindAll(cd => cd.card.holder == null || controller.handHolder.cardHolder.hasCard(cd.card));
            int randIndex1 = Random.Range(0, unplayedCardList.Count);
            CardDisplayer cd = unplayedCardList[randIndex1];
        if (cd && controller.canPickupCard(cd))
        {
            List<CardHolderDisplayer> emptyHolderList = holders.FindAll((holder) => holder.cardHolder.CardCount == 0);
            if (emptyHolderList.Count > 0)
            {
                int randIndex = Random.Range(0, emptyHolderList.Count);
                CardHolderDisplayer ch = emptyHolderList[randIndex];
                if (controller.canPlaceCardAt(cd, ch.cardHolder))
                {
                    controller.placeCard(cd, ch.cardHolder);
                }
            }
        }
        }
    }

    private bool canPlayAnyCard()
    {
        return controller.CardDisplayerList.Any(c =>
            //not on the board
            (c.card.holder == null || controller.handHolder.cardHolder.hasCard(c.card))
            //can be played
            && controller.canPickupCard(c)
            //can be placed at any position
            && holders.Any(h => controller.canPlaceCardAt(c, h.cardHolder))
            );
    }
}
