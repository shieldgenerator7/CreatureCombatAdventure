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
        //get moves
        List<Move> moves = new List<Move>();
        controller.Wrangler.cardList.ForEach(card =>
        {
            if (!card)
            {
                throw new Exception($"Invalid card! {card}");
            }
            controller.Wrangler.cardHolders.ForEach(holder =>
            {
                if (!holder)
                {
                    throw new Exception($"Invalid holder! {holder}");
                }
                if (holder.CardCount >= holder.limit && holder != card.holder) { return; }
                if (!controller.Wrangler.canPlaceCardAt(card, holder)) { return; }
                Move move = new Move(card, holder);
                if (move.type == Move.Type.PASS) { return; }
                moves.Add(move);
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
