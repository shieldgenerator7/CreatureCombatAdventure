using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public float cardScale = 0.25f;
    public List<CreatureCard> cardList;
    public WranglerController owner;
    public int limit = 1;

    public SpriteRenderer highlight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public CreatureCard Card => cardList.FirstOrDefault();
    public int CardCount => cardList.Count;

    public bool hasCard(CreatureCard card)
    {
        return cardList.Contains(card);
    }

    public void checkDrop(CardDisplayer cardDisplayer, Vector2 mousePos)
    {

    }

    public bool canAcceptCard(CardDisplayer card)
    {
        return cardList.Count < limit;
    }

    public void acceptDrop(CardDisplayer cardDisplayer)
    {
        if (cardDisplayer.holder != null)
        {
            cardDisplayer.holder.removeCard(cardDisplayer);
        }
        cardDisplayer.transform.position = transform.position;
        cardDisplayer.transform.localScale = Vector3.one * cardScale;
        cardDisplayer.transform.rotation = transform.rotation;
        cardList.Add(cardDisplayer.card);
        cardDisplayer.holder = this;
    }

    public void acceptMouseHover(bool hover)
    {
        highlight.enabled = hover;
    }

    public void removeCard(CardDisplayer card)
    {
        cardList.Remove(card.card);
    }

    public void Update()
    {
    }
}
