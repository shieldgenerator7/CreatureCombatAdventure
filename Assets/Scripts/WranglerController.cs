using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WranglerController : MonoBehaviour
{
    private Wrangler player;
    public Wrangler Wrangler
    {
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

        wrangler.OnCardAdded += (card) =>
        {
            CardDisplayer cd = createCard(card);
            //proc delegate again
            //wrangler.handHolder.acceptDrop(card);
            //deactivate card
            cd.gameObject.SetActive(false);
            //move card to middle of screen
            cd.cardLayer = 10;
            cd.sortingLayer = "Hand";
            cd.transform.localScale = Vector3.one * 0.3f;
            //queue anim to show it
            ShowUIAnimation.showObject(cd.gameObject, true);
        };
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
            createCard(card);
        });
    }
    private CardDisplayer createCard(Card card)
    {
        GameObject go = Instantiate(cardDisplayerPrefab, Bin.Transform);
        CardDisplayer cd = go.GetComponent<CardDisplayer>();
        cd.card = card;
        card.owner = this.player;
        cardDisplayerList.Add(cd);
        return cd;
    }

    public static WranglerController Find(Wrangler wrangler)
    {
        return FindObjectsByType<WranglerController>(FindObjectsSortMode.None).FirstOrDefault(wc => wc.player == wrangler);
    }
}
