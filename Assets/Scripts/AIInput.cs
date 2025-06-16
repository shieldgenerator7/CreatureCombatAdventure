using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIInput : WranglerInput
{

    public List<CardHolder<Card>> holders;

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
            //TODO: put back in
        List<CardDisplayer> unplayedCardList = controller.CardDisplayerList.FindAll(cd => cd.holder == null);// || controller.handHolderList.Contains(cd.holder));
            int randIndex1 = Random.Range(0, unplayedCardList.Count);
            CardDisplayer cd = unplayedCardList[randIndex1];
        if (cd && controller.canPickupCard(cd))
        {
            List<CardHolder<Card>> emptyHolderList = holders.FindAll((holder) => holder.CardCount == 0);
            if (emptyHolderList.Count > 0)
            {
                int randIndex = Random.Range(0, emptyHolderList.Count);
                CardHolder<Card> ch = emptyHolderList[randIndex];
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
            //TODO: put back in
            (c.holder == null)// || controller.handHolderList.Contains(c.holder))
            //can be played
            && controller.canPickupCard(c)
            //can be placed at any position
            && holders.Any(h => controller.canPlaceCardAt(c, h))
            );
    }
}
