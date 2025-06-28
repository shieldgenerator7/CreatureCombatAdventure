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
            GameObject go = Instantiate(cardDisplayerPrefab, Bin.Transform);
            CardDisplayer cd = go.GetComponent<CardDisplayer>();
            cd.card = card;
            card.owner = this.player;
            cardDisplayerList.Add(cd);
        });
    }

    public static WranglerController Find(Wrangler wrangler)
    {
        return FindObjectsByType<WranglerController>(FindObjectsSortMode.None).FirstOrDefault(wc => wc.player == wrangler);
    }
}
