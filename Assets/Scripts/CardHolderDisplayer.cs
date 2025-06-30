using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardHolderDisplayer : MonoBehaviour
{
    [Header("Components")]
    public List<SpriteRenderer> highlights;
    public SpriteMask smHighlight;
    [Header("Settings")]
    public float spreadWidth = 0;
    public Vector2 spreadDir = Vector2.zero;
    public float maxSpreadBuffer = 2;
    public float mouseOverPopupDistance = 2;
    public int baseLayer = 0;
    public bool instantDrop = false;

    private CardHolder cardHolder;
    public CardHolder CardHolder => cardHolder;

    private string SortingLayer => (cardHolder.isHand) ? "Hand" : "Card";

    public void init(CardHolder cardHolder)
    {
        this.cardHolder = cardHolder;
        cardHolder.OnCardDropped += listenForDrop;
        cardHolder.OnCardRemoved += listenForRemove;
        highlights.ForEach(highlight => highlight.sortingLayerName = SortingLayer);
        smHighlight.frontSortingLayerID = UnityEngine.SortingLayer.NameToID(SortingLayer);
        smHighlight.backSortingLayerID = UnityEngine.SortingLayer.NameToID(SortingLayer);
        acceptMouseHover(false);
    }

    private void listenForDrop(Card card)
    {
        CardDisplayer cd = CardDisplayer.Find(card);
        //possible that the card was added before the display created it
        if (!cd)
        {
            return;
        }
        //
        acceptDrop(cd);
    }

    private void listenForRemove(Card card)
    {
        CardDisplayer cd = CardDisplayer.Find(card);
        //possible that the card was added before the display created it
        if (!cd)
        {
            return;
        }
        //
        removeCard(cd);
    }

    private void acceptDrop(CardDisplayer cardDisplayer)
    {
        if (instantDrop)
        {
            cardDisplayer.transform.position = transform.position;
            cardDisplayer.transform.localScale = transform.localScale;
            cardDisplayer.transform.rotation = transform.rotation;
            cardDisplayer.OnMousedOver += listenForMouseOver;
            layoutCards();
        }
        else
        {
            MoveUIAnimation move = MoveUIAnimation.moveTo(cardDisplayer.gameObject, transform);
            move.OnFinished += () =>
            {
                cardDisplayer.OnMousedOver += listenForMouseOver;
                layoutCards();
            };
        }
    }

    private void layoutCards()
    {
        if (cardHolder.CardCount == 0) { return; }
        //TODO: consolidate these two very similar if blocks
        if (spreadWidth > 0)
        {
            float startX = -spreadWidth / 2;
            float spreadBuffer = Mathf.Min(spreadWidth / cardHolder.CardCount, maxSpreadBuffer);
            for (int i = 0; i < cardHolder.CardCount; i++)
            {
                CardDisplayer cd = CardDisplayer.Find(cardHolder.cardList[i]);
                cd.transform.position = (Vector2)transform.position + new Vector2(startX + spreadBuffer * i, 0);
                cd.cardLayer = baseLayer + i + 1;
                if (cd.MousedOver)
                {
                    if (cardHolder.isHand)
                    {
                        cd.cardLayer = baseLayer + 50;
                        cd.transform.position += Vector3.up * mouseOverPopupDistance;
                        cd.acceptHover(false);
                    }
                    else
                    {
                        cd.acceptHover(true);
                    }
                }
                cd.sortingLayer = SortingLayer;
                cd.updateDisplay();
                //return layer to normal so that moused over cards dont prohibit their neighbors from being moused over
                cd.cardLayer = baseLayer + i + 1;
            }
        }
        else if (spreadDir != Vector2.zero)
        {
            Vector2 startPos = transform.position;
            Vector2 normDir = spreadDir.normalized;
            float spreadBuffer = Mathf.Min(spreadDir.magnitude / cardHolder.CardCount, maxSpreadBuffer);
            for (int i = 0; i < cardHolder.CardCount; i++)
            {
                CardDisplayer cd = CardDisplayer.Find(cardHolder.cardList[i]);
                cd.transform.position = (Vector2)transform.position + (normDir * (spreadBuffer * i));
                cd.cardLayer = baseLayer + i + 1;
                if (cd.MousedOver)
                {
                    if (cardHolder.isHand)
                    {
                        cd.cardLayer = baseLayer + 50;
                        cd.transform.position += Vector3.up * mouseOverPopupDistance;
                        cd.acceptHover(false);
                    }
                    else
                    {
                        cd.acceptHover(true);
                    }
                }
                cd.sortingLayer = SortingLayer;
                cd.updateDisplay();
                //return layer to normal so that moused over cards dont prohibit their neighbors from being moused over
                cd.cardLayer = baseLayer + i + 1;
            }
        }
        //TODO: functionality to lay them out vertically
    }

    private void listenForMouseOver(CardDisplayer cd, bool mousedOver)
    {
        layoutCards();
    }


    public void acceptMouseHover(bool hover)
    {
        highlights.ForEach(highlight => highlight.enabled = hover);
    }

    public void removeCard(CardDisplayer card)
    {
        card.OnMousedOver -= listenForMouseOver;
        layoutCards();
    }

    public static CardHolderDisplayer Find(CardHolder holder)
    {
        return FindObjectsByType<CardHolderDisplayer>(FindObjectsSortMode.None).FirstOrDefault(chd => chd.cardHolder == holder);
    }
}
