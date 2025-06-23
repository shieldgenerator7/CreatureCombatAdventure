using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIInput : WranglerInput
{
    [NonSerialized]
    public AIBrain aiBrain;

    public override void processTurn()
    {
        base.processTurn();
        //early exit: game is already won
        if (FindAnyObjectByType<GameManager>().GameOver) { controller.Wrangler.skipTurn(); return; }
            //get moves
            List<Move> moves = new List<Move>();
            controller.Wrangler.cardList.ForEach(card =>
            {//
                controller.Wrangler.cardHolders.ForEach(holder =>
                {//
                    if (holder.CardCount >= holder.limit && holder != card.holder) { return; }
                    if (!controller.Wrangler.canPlaceCardAt(card, holder)) { return; }
                    moves.Add(new Move(card, holder));
                });
            });
        if (moves.Count == 0)
        {
            controller.Wrangler.skipTurn();
            return;
        }
            //ask ai to pick a move to do
            Move move = aiBrain.pickMove(moves);
        if (!move.Valid)
        {
            throw new Exception($"Invalid move! {move}");
        }
            controller.Wrangler.placeCard(move.card, move.holder);
    }
}
