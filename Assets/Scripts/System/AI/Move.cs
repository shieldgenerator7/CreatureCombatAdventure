using System.Linq;
using UnityEngine;

public struct Move
{
    public Card card;
    public CardHolder holder;

    public Card opposingCard;
    public CardHolder opposingHolder;

    /// <summary>
    /// Does this move cause this wrangler to win/lose RPS in this lane, when it wouldn't before?
    /// </summary>
    public WinState winRPS;
    /// <summary>
    /// Does this moves cause this wrangler to win/lose (raw) Power in this lane, when it wouldn't before?
    /// </summary>
    public WinState winPowerRaw;

    public CardState cardState;

    public enum Type
    {
        PASS,
        PLAY,
        MOVE,
        REORDER,
        ACTIVATE,
    }
    public Type type;

    public bool isMovingIntoEmpty;
    public bool isMoveFillsCapacity;

    public bool Valid => type != Type.PASS;

    public Move(Card card, CardHolder holder)
    {
        if (!card || !holder)
        {
            type = Type.PASS;
            this.card = null;
            this.holder = null;
            opposingCard = null;
            opposingHolder = null;
            winRPS = WinState.NONE;
            winPowerRaw = WinState.NONE;
            cardState = new CardState(null);
            isMovingIntoEmpty = false;
            isMoveFillsCapacity = false;
            return;
        }

        //Assumption: never trying to move a card back into the hand
        //Asumption: moving a card puts it at the back of the list for that lane
        this.card = card;
        this.holder = holder;

        opposingHolder = holder.opposingHolder;
        opposingCard = opposingHolder.Card;

        if (holder.CardCount > 0)
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
            if (holderPower + cardPower > holderPowerOpposing)
            {
                winPowerRaw = WinState.WIN;
            }
            else if (holderPower + cardPower == holderPowerOpposing)
            {
                winPowerRaw = WinState.NONE;
            }
            else
            {
                winPowerRaw = WinState.LOSE;
            }
        }
        //If it's currently losing,
        else if (holderPower < holderPowerOpposing)
        {
            if (holderPower + cardPower > holderPowerOpposing)
            {
                winPowerRaw = WinState.WIN;
            }
            else if (holderPower + cardPower == holderPowerOpposing)
            {
                winPowerRaw = WinState.DRAW;
            }
            else
            {
                winPowerRaw = WinState.NONE;
            }
        }
        //If it's currently winning,
        else if (holderPower > holderPowerOpposing)
        {
            if (holderPower + cardPower > holderPowerOpposing)
            {
                winPowerRaw = WinState.NONE;
            }
            else if (holderPower + cardPower == holderPowerOpposing)
            {
                winPowerRaw = WinState.DRAW;
            }
            else
            {
                winPowerRaw = WinState.LOSE;
            }
        }
        //shouldn't be possible to reach this state
        else
        {
            throw new System.Exception($"This shouldn't be possible. {holderPower} + {cardPower} == {holderPowerOpposing}?");
        }

        cardState = new CardState(card);

        if (card.holder.isHand)
        {
            type = Type.PLAY;
        }
        else if (card.holder != holder)
        {
            type = Type.MOVE;
        }
        else if (card.holder.cardList.Last() != card)
        {
            type = Type.REORDER;
        }
        //TODO: implement this
        //else if (true)
        //{
        //    type = Type.ACTIVATE;
        //}
        else
        {
            type = Type.PASS;
        }

        bool movingToNewHolder = (type == Type.PLAY || type == Type.MOVE);
        isMovingIntoEmpty = movingToNewHolder && holder.CardCount == 0;
        isMoveFillsCapacity = movingToNewHolder && holder.CardCount == holder.limit - 1;

    }
}
