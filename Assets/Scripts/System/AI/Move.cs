using System.Linq;
using UnityEngine;

public struct Move
{
    public Card card;
    public CardHolder holder;

    public Card opposingCard;
    public CardHolder opposingHolder;

    public bool beatsOpposingRPS;
    public bool beatsOpposingPower;

    public CardHolder holderCurrent;

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

    public Move(Card card, CardHolder holder)
    {
        //Assumption: never trying to move a card back into the hand
        this.card = card;
        this.holder = holder;

        opposingHolder = holder.opposingHolder;
        opposingCard = opposingHolder.cardList.FirstOrDefault();

        beatsOpposingRPS = holder.arena.data.rpsSetData.beats(
            card.data.rps, 
            opposingCard?.data.rps ?? RockPaperScissors.NONE
            );
        beatsOpposingPower = holder.Power + card.data.power > opposingHolder.Power;

        holderCurrent = card.holder;

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
