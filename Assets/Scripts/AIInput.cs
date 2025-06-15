using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIInput : WranglerInput
{

    public List<CardHolder> holders;

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
        List<CardDisplayer> unplayedCardList = controller.CardDisplayerList.FindAll(cd=>cd.holder == null || controller.handHolderList.Contains(cd.holder));
            int randIndex1 = Random.Range(0, unplayedCardList.Count);
            CardDisplayer cd = unplayedCardList[randIndex1];
        if (cd && controller.canPickupCard(cd))
        {
            List<CardHolder> emptyHolderList = holders.FindAll((holder) => holder.card == null);
            if (emptyHolderList.Count > 0)
            {
                int randIndex = Random.Range(0, emptyHolderList.Count);
                CardHolder ch = emptyHolderList[randIndex];
                if (controller.canPlaceCardAt(cd, ch))
                {
                    controller.placeCard(cd, ch);
                }
            }
        }
        }
    }

    private bool canPlayAnyCard()
    {
        return controller.CardDisplayerList.Any(c =>
            //not on the board
            (c.holder == null || controller.handHolderList.Contains(c.holder))
            //can be played
            && controller.canPickupCard(c)
            //can be placed at any position
            && holders.Any(h => controller.canPlaceCardAt(c, h))
            );
    }
}
