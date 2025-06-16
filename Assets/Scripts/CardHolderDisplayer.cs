using UnityEngine;

public class CardHolderDisplayer : MonoBehaviour
{
    public SpriteRenderer highlight;
    public float cardScale = 0.25f;

    public CardHolder<Card> cardHolder;

    public void acceptDrop(CardDisplayer cardDisplayer)
    {
        if (cardDisplayer.holder != null)
        {
            cardDisplayer.holder.removeCard(cardDisplayer);
        }
        cardDisplayer.transform.position = transform.position;
        cardDisplayer.transform.localScale = Vector3.one * cardScale;
        cardDisplayer.transform.rotation = transform.rotation;
        cardHolder.cardList.Add(cardDisplayer.card);
        cardDisplayer.holder = this;
    }


    public void acceptMouseHover(bool hover)
    {
        highlight.enabled = hover;
    }

    public void removeCard(CardDisplayer card)
    {
        cardHolder.Remove(card.card);
    }
}
