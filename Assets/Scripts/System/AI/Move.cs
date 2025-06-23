using System.Linq;
using UnityEngine;

public struct Move
{
    public Card card;
    public CardHolder holder;

    public Card opposingCard;
    public CardHolder opposingHolder;

    /// <summary>
    /// True if this move causes this wrangler to win RPS in this lane, when it wouldn't before
    /// </summary>
    public bool beatsOpposingRPS;
    /// <summary>
    /// True if this moves causes this wrangler to win (raw) Power in this lane, when it wouldn't before
    /// </summary>
    public bool beatsOpposingPowerRaw;

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
            beatsOpposingRPS = false;
            beatsOpposingPowerRaw = false;
            cardState = new CardState(card);
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

        beatsOpposingRPS = holder.CardCount == 0 && holder.arena.data.rpsSetData.beats(
            card.data.rps, 
            opposingCard?.data.rps ?? RockPaperScissors.NONE
            );
        int holderPower = holder.PowerRaw;
        int holderPowerOpposing = opposingHolder.PowerRaw;
        beatsOpposingPowerRaw = holderPower + card.data.power > holderPowerOpposing && holderPower <= holderPowerOpposing;

        cardState = new CardState(card);

        type = Type.PASS;
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

        bool movingToNewHolder = (type == Type.PLAY || type == Type.MOVE);
        isMovingIntoEmpty = movingToNewHolder && holder.CardCount == 0;
        isMoveFillsCapacity = movingToNewHolder && holder.CardCount == holder.limit - 1;

    }
}
