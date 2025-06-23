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

    public bool losesRPS;
    public bool losesPower;

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
            beatsOpposingRPS = false;
            beatsOpposingPowerRaw = false;
            isFirst = false;
            isLast = false;
            losesRPS = false;
            losesPower = false;
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
            beatsOpposingRPS = false;
            beatsOpposingPowerRaw = false;
            losesRPS = false;
            losesPower = false;
            winningPowerLane = false;
            losingPowerLane = false;
            return;
        }
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


        losesRPS = isFirst && holder.arena.data.rpsSetData.beats(
            opposingCard?.data.rps ?? RockPaperScissors.NONE,
            card.data.rps
            );
        //TODO: rework losesPower calculation to be smarter (probably need to account for abilities)
        losesPower = holderPower < holderPowerOpposing && holderPower - card.data.power >= holderPowerOpposing;
        winningPowerLane = holderPower > holderPowerOpposing;
        losingPowerLane = holderPower < holderPowerOpposing;
    }
}
