using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public float cardScale = 0.25f;
    public CreatureCard card;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void checkDrop(CardDisplayer cardDisplayer, Vector2 mousePos)
    {

    }

    public void acceptDrop(CardDisplayer cardDisplayer) { 
        if (cardDisplayer.holder != null)
        {
            cardDisplayer.holder.card = null;
        }
        cardDisplayer.transform.position = transform.position;
        cardDisplayer.transform.localScale = Vector3.one * cardScale;
        card = cardDisplayer.card;
        cardDisplayer.holder = this;
    }

    public void Update()
    {
    }
}
