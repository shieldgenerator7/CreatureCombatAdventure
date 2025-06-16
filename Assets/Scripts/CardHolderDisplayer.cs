using UnityEngine;

public class CardHolderDisplayer : MonoBehaviour
{
    public SpriteRenderer highlight;
    public float cardScale = 0.25f;

    public CardHolder cardHolder;

    public void acceptDrop(CardDisplayer cardDisplayer)
    {
        //TODO: turn this into a listener maybe, listening for when a card is dropped
        if (cardDisplayer.card.holder != null)
        {
            cardDisplayer.card.holder.removeCard(cardDisplayer.card);
        }
        cardDisplayer.transform.position = transform.position;
        cardDisplayer.transform.localScale = Vector3.one * cardScale;
        cardDisplayer.transform.rotation = transform.rotation;
        cardHolder.cardList.Add(cardDisplayer.card);
        cardDisplayer.card.holder = this.cardHolder;
    }


    public void acceptMouseHover(bool hover)
    {
        highlight.enabled = hover;
    }

    public void removeCard(CardDisplayer card)
    {
        cardHolder.removeCard(card.card);
    }
}
