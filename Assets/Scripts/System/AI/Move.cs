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

    public bool isMovePlay;
    public bool isMoveMove;
    public bool isMoveReorder;
    public bool isMoveActivate;

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

        isMovePlay = card.holder.isHand;
        isMoveMove = !card.holder.isHand && card.holder != holder;
        isMoveReorder = !card.holder.isHand && card.holder == holder && card.holder.cardList.Last() != card;
        //TODO: implement this
        isMoveActivate = false;

        isMovingIntoEmpty = (isMovePlay || isMoveMove) && holder.CardCount == 0;
        isMoveFillsCapacity = (isMovePlay || isMoveMove) && holder.CardCount == holder.limit - 1;

    }
}
