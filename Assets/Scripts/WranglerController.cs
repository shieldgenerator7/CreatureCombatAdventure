using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WranglerController : MonoBehaviour
{
    private Wrangler player;
    public Wrangler Wrangler {
        get => player;
        private set
        {
            player = value;
        }
    }

    public GameObject cardDisplayerPrefab;
    private List<CardDisplayer> cardDisplayerList = new List<CardDisplayer>();

    public void init(Wrangler wrangler)
    {
        Wrangler = wrangler;

        createCards();
    }


    private void createCards()
    {
        cardDisplayerList.ForEach(card =>
        {
            Destroy(card.gameObject);
        });
        cardDisplayerList.Clear();
        player.cardList.ForEach(card =>
        {
            GameObject go = Instantiate(cardDisplayerPrefab);
            CardDisplayer cd = go.GetComponent<CardDisplayer>();
            cd.card = card;
            card.owner = this.player;
            cardDisplayerList.Add(cd);
        });
    }

    public bool canPickupCard(Card card)
    {
        return card.owner == this.player;
    }

    public bool canPlaceCardAt(Card card, CardHolder cardHolder)
    {
        return card.owner == this.player && cardHolder.owner == this.player
            && (cardHolder.canAcceptCard(card) || cardHolder.hasCard(card));
    }

    public void placeCard(Card card, CardHolder holder)
    {
        //a move is a fake move if it moves to the same holder and is either a hand, or the card was already the last in the list
        bool fakeMove = card.holder == holder && (holder.isHand || card.holder.cardList.Last() == card);
        if (card.holder)
        {
            card.holder.removeCard(card);
        }
        holder.acceptDrop(card);
        OnCardPlaced?.Invoke();
        if (!fakeMove)
        {
        OnTurnTaken?.Invoke();
        }
    }
    public event Action OnCardPlaced;
    public event Action OnTurnTaken;

    public static WranglerController Find(Wrangler wrangler)
    {
        return FindObjectsByType<WranglerController>(FindObjectsSortMode.None).FirstOrDefault(wc => wc.player == wrangler);
    }
}
