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
    /// Is this card causing this wrangler to win/lose power (raw) in this lane, when it wouldn't otherwise?
    /// </summary>
    public WinState winPowerRaw;

    /// <summary>
    /// Is the lane currently winning (raw) power?
    /// </summary>
    public WinState winPowerRawLane;

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
            winPowerRaw = WinState.NONE;
            isFirst = false;
            isLast = false;
            winPowerRawLane = WinState.NONE;
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
            winPowerRaw = WinState.NONE;
            winPowerRawLane = WinState.NONE;
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

        int cardPower = card.data.power;
        int holderPower = holder.PowerRaw;
        int holderPowerOpposing = opposingHolder.PowerRaw;
        //If it's currently a draw,
        if (holderPower == holderPowerOpposing)
        {
            winPowerRawLane = WinState.DRAW;
            //TODO: rework losesPower calculation to be smarter (probably need to account for abilities)
            if (holderPower - cardPower > holderPowerOpposing)
            {
                winPowerRaw = WinState.DRAW;
            }
            else if (holderPower - cardPower == holderPowerOpposing)
            {
                winPowerRaw = WinState.NONE;
            }
            else
            {
                winPowerRaw = WinState.DRAW;
            }
        }
        //If it's currently losing,
        else if (holderPower < holderPowerOpposing)
        {
            winPowerRawLane = WinState.LOSE;
            //TODO: rework losesPower calculation to be smarter (probably need to account for abilities)
            if (holderPower - cardPower > holderPowerOpposing)
            {
                winPowerRaw = WinState.LOSE;
            }
            else if (holderPower - cardPower == holderPowerOpposing)
            {
                winPowerRaw = WinState.LOSE;
            }
            else
            {
                winPowerRaw = WinState.NONE;
            }
        }
        //If it's currently winning,
        else if (holderPower > holderPowerOpposing)
        {
            winPowerRawLane = WinState.WIN;
            if (holderPower - cardPower > holderPowerOpposing)
            {
                winPowerRaw = WinState.NONE;
            }
            else if (holderPower - cardPower == holderPowerOpposing)
            {
                winPowerRaw = WinState.WIN;
            }
            else
            {
                winPowerRaw = WinState.WIN;
            }
        }
        //shouldn't be possible to reach this state
        else
        {
            throw new System.Exception($"This shouldn't be possible. {holderPower} + {cardPower} == {holderPowerOpposing}?");
        }

    }
}
