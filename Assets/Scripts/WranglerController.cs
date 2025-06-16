using System;
using System.Collections.Generic;
using UnityEngine;

public class WranglerController : MonoBehaviour
{
    public Wrangler player;

    public GameObject cardDisplayerPrefab;
    private List<CardDisplayer> cardDisplayerList = new List<CardDisplayer>();

    public CardHolderDisplayer handHolder;
    public List<CardHolderDisplayer> holderList;

    private void Awake()
    {
        //holders
        handHolder.init(player.handHolder);
        player.handHolder.owner = player;
        for(int i =0; i < holderList.Count; i++)
        {
            holderList[i].init(player.cardHolders[i]);
            player.cardHolders[i].owner = player;
        }

        //cards
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
            handHolder.CardHolder.acceptDrop(card);

            cardDisplayerList.Add(cd);
        });
    }

    public List<CardDisplayer> CardDisplayerList =>cardDisplayerList;

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
        if (card.holder)
        {
            card.holder.removeCard(card);
        }
        holder.acceptDrop(card);
        OnCardPlaced?.Invoke();
        OnTurnTaken?.Invoke();
    }
    public event Action OnCardPlaced;
    public event Action OnTurnTaken;
}
