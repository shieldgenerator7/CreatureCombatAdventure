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
    public bool beatingOpposingRPS;
    /// <summary>
    /// True if this wrangler is winning power (raw) in this lane, and removing this card would cause that not to be true
    /// </summary>
    public bool beatingOpposingPowerRaw;

    public bool losingRPS;
    public bool losingPower;

    public bool winningPowerLane;
    public bool losingPowerLane;

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
            beatingOpposingRPS = false;
            beatingOpposingPowerRaw = false;
            isFirst = false;
            isLast = false;
            losingRPS = false;
            losingPower = false;
            winningPowerLane = false;
            losingPowerLane = false;
            return;
        }

        this.card = card;
        holder = card.holder;

        if (holder.isHand)
        {
            opposingHolder = null;
            opposingCard = null;
            isFirst = false;
            isLast = false;
            beatingOpposingRPS = false;
            beatingOpposingPowerRaw = false;
            losingRPS = false;
            losingPower = false;
            winningPowerLane = false;
            losingPowerLane = false;
            return;
        }
        opposingHolder = holder.opposingHolder;
        opposingCard = opposingHolder.Card;

        isFirst = !holder.isHand && holder.Card == card;
        isLast = !holder.isHand && holder.cardList.Last() == card;

        beatingOpposingRPS = isFirst && holder.arena.data.rpsSetData.beats(
            card.data.rps,
            opposingCard?.data.rps ?? RockPaperScissors.NONE
            );
        int holderPower = holder.PowerRaw;
        int holderPowerOpposing = opposingHolder.PowerRaw;
        beatingOpposingPowerRaw = holderPower > holderPowerOpposing && holderPower - card.data.power <= holderPowerOpposing;


        losingRPS = isFirst && holder.arena.data.rpsSetData.beats(
            opposingCard?.data.rps ?? RockPaperScissors.NONE,
            card.data.rps
            );
        //TODO: rework losesPower calculation to be smarter (probably need to account for abilities)
        losingPower = holderPower < holderPowerOpposing && holderPower - card.data.power >= holderPowerOpposing;
        winningPowerLane = holderPower > holderPowerOpposing;
        losingPowerLane = holderPower < holderPowerOpposing;
    }
}
