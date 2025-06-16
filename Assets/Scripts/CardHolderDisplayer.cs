using System.Linq;
using UnityEngine;

public class CardHolderDisplayer : MonoBehaviour
{
    public SpriteRenderer highlight;


    private CardHolder cardHolder;
    public CardHolder CardHolder => cardHolder;

    public void init(CardHolder cardHolder)
    {
        this.cardHolder = cardHolder;
        cardHolder.OnCardDropped += listenForDrop;
    }

    private void listenForDrop(Card card)
    {
        CardDisplayer cd = CardDisplayer.Find(card);
        acceptDrop(cd);
    }

    private void acceptDrop(CardDisplayer cardDisplayer)
    {
        cardDisplayer.transform.position = transform.position;
        cardDisplayer.transform.localScale = transform.localScale;
        cardDisplayer.transform.rotation = transform.rotation;
        //TODO: make function to layout cards in hand / other holder
    }


    public void acceptMouseHover(bool hover)
    {
        highlight.enabled = hover;
    }

    public void removeCard(CardDisplayer card)
    {
        cardHolder.removeCard(card.card);
    }

    public static CardHolderDisplayer Find(CardHolder holder)
    {
        return FindObjectsByType<CardHolderDisplayer>(FindObjectsSortMode.None).FirstOrDefault(chd => chd.cardHolder == holder);
    }
}
