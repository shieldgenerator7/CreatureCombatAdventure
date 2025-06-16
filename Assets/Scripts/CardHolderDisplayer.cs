using System.Linq;
using UnityEngine;

public class CardHolderDisplayer : MonoBehaviour
{
    public SpriteRenderer highlight;
    public float spreadWidth = 0;
    public float maxSpreadBuffer = 2;
    public float mouseOverPopupDistance = 2;

    private CardHolder cardHolder;
    public CardHolder CardHolder => cardHolder;

    public void init(CardHolder cardHolder)
    {
        this.cardHolder = cardHolder;
        cardHolder.OnCardDropped += listenForDrop;
        cardHolder.OnCardRemoved += listenForRemove;
    }

    private void listenForDrop(Card card)
    {
        CardDisplayer cd = CardDisplayer.Find(card);
        acceptDrop(cd);
    }

    private void listenForRemove(Card card)
    {
        CardDisplayer cd = CardDisplayer.Find(card);
        removeCard(cd);
    }

    private void acceptDrop(CardDisplayer cardDisplayer)
    {
        cardDisplayer.transform.position = transform.position;
        cardDisplayer.transform.localScale = transform.localScale;
        cardDisplayer.transform.rotation = transform.rotation;
        cardDisplayer.OnMousedOver += listenForMouseOver;
        layoutCards();
    }

    private void layoutCards()
    {
        if (cardHolder.CardCount == 0) { return; }
        if (spreadWidth > 0)
        {
            float startX = -spreadWidth / 2;
            float spreadBuffer = Mathf.Min(spreadWidth / cardHolder.CardCount, maxSpreadBuffer);
            for (int i = 0; i < cardHolder.CardCount; i++)
            {
                CardDisplayer cd = CardDisplayer.Find(cardHolder.cardList[i]);
                cd.transform.position = (Vector2)transform.position + new Vector2(startX + spreadBuffer * i, 0);
                cd.cardLayer = i + 1;
                if (cd.MousedOver)
                {
                    cd.cardLayer = 50;
                    cd.transform.position += Vector3.up * mouseOverPopupDistance;
                }
                cd.updateDisplay();
            }
        }
        //TODO: functionality to lay them out vertically
    }

    private void listenForMouseOver(CardDisplayer cd, bool mousedOver)
    {
        if (spreadWidth > 0)
        {
            layoutCards();
        }
    }


    public void acceptMouseHover(bool hover)
    {
        highlight.enabled = hover;
    }

    public void removeCard(CardDisplayer card)
    {
        card.OnMousedOver -= listenForMouseOver;
    }

    public static CardHolderDisplayer Find(CardHolder holder)
    {
        return FindObjectsByType<CardHolderDisplayer>(FindObjectsSortMode.None).FirstOrDefault(chd => chd.cardHolder == holder);
    }
}
