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
    /// Is this card causing this wrangler to win/lose RPS in this lane?
    /// </summary>
    public WinState winRPS;
    /// <summary>
    /// True if this wrangler is winning power (raw) in this lane, and removing this card would cause that not to be true
    /// </summary>
    public bool beatingOpposingPowerRaw;

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
            winRPS = WinState.NONE;
            beatingOpposingPowerRaw = false;
            isFirst = false;
            isLast = false;
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
            winRPS = WinState.NONE;
            beatingOpposingPowerRaw = false;
            losingPower = false;
            winningPowerLane = false;
            losingPowerLane = false;
            return;
        }
        opposingHolder = holder.opposingHolder;
        opposingCard = opposingHolder.Card;

        isFirst = !holder.isHand && holder.Card == card;
        isLast = !holder.isHand && holder.cardList.Last() == card;

        if (!isFirst)
        {
            winRPS = WinState.NONE;
        }
        else if (holder.arena.data.rpsSetData.beats(
            card.data.rps,
            opposingCard?.data.rps ?? RockPaperScissors.NONE
            ))
        {
            winRPS = WinState.WIN;
        }
        else if (holder.arena.data.rpsSetData.beats(
            opposingCard?.data.rps ?? RockPaperScissors.NONE,
            card.data.rps
            ))
        {
            winRPS = WinState.LOSE;
        }
        else if (opposingCard != null)
        {
            winRPS = WinState.DRAW;
        }
        else
        {
            winRPS = WinState.NONE;
        }

        int holderPower = holder.PowerRaw;
        int holderPowerOpposing = opposingHolder.PowerRaw;
        beatingOpposingPowerRaw = holderPower > holderPowerOpposing && holderPower - card.data.power <= holderPowerOpposing;


        //TODO: rework losesPower calculation to be smarter (probably need to account for abilities)
        losingPower = holderPower < holderPowerOpposing && holderPower - card.data.power >= holderPowerOpposing;
        winningPowerLane = holderPower > holderPowerOpposing;
        losingPowerLane = holderPower < holderPowerOpposing;
    }
}
