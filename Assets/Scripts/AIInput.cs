using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIInput : WranglerInput
{
    public AIBrain aiBrain;

    public override void processTurn()
    {
        base.processTurn();
        //try playing cards
        if (canPlayAnyCard()) {
            //get moves
            List<Move> moves = new List<Move>();
            controller.Wrangler.cardList.ForEach(card =>
            {//
                controller.Wrangler.cardHolders.ForEach(holder =>
                {//
                    if (holder.CardCount >= holder.limit && holder != card.holder) { return; }
                    if (!controller.canPlaceCardAt(card, holder)) { return; }
                    moves.Add(new Move(card, holder));
                });
            });
            //ask ai to pick a move to do
            Move move = aiBrain.pickMove(moves);
            controller.placeCard(move.card, move.holder);
        }//
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
