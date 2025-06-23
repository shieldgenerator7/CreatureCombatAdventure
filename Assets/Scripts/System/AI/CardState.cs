using System;
using System.Linq;
using UnityEngine;

public struct CardState
{
    public Card card;
    public CardHolder holder;

    public Card opposingCard;
    public CardHolder opposingHolder;

    /// <summary>
    /// True if this card is causing this wrangler to win RPS in this lane
    /// </summary>
    public bool beatsOpposingRPS;
    /// <summary>
    /// True if this wrangler is winning power (raw) in this lane, and removing this card would cause that not to be true
    /// </summary>
    public bool beatsOpposingPowerRaw;

    public bool isFirst;
    public bool isLast;

    public bool Valid => card != null;

    public CardState(Card card)
    {
        if (!card)
        {
            this.card = null;
            this.holder = null;
            opposingCard = null;
            opposingHolder = null;
            beatsOpposingRPS = false;
            beatsOpposingPowerRaw = false;
            isFirst = false;
            isLast = false;
            return;
        }

        this.card = card;
        holder = card.holder;

        opposingHolder = holder.opposingHolder;
        opposingCard = opposingHolder.Card;

        isFirst = !holder.isHand && holder.Card == card;
        isLast = !holder.isHand && holder.cardList.Last() == card;

        beatsOpposingRPS = isFirst && holder.arena.data.rpsSetData.beats(
            card.data.rps,
            opposingCard?.data.rps ?? RockPaperScissors.NONE
            );
        int holderPower = holder.PowerRaw;
        int holderPowerOpposing = opposingHolder.PowerRaw;
        beatsOpposingPowerRaw = holderPower > holderPowerOpposing && holderPower - card.data.power <= holderPowerOpposing;
    }
}
