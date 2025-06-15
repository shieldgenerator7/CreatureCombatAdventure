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
        //find card
        CardDisplayer cd = controller.CardDisplayerList.FirstOrDefault(cd=>cd.holder == null);
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
