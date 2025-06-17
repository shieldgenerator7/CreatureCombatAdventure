using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CardDisplayer : MonoBehaviour
{
    public Card card;

    public SpriteRenderer srBack;
    public SpriteRenderer srImage;
    public SpriteRenderer srFrame;
    public SpriteMask smFrame;

    public Canvas cvs;
    public TMP_Text txtName;
    public TMP_Text txtCost;
    public TMP_Text txtRPS;
    public TMP_Text txtPower;

    public string sortingLayer = "Card";

    private bool mousedOver = false;
    public bool MousedOver
    {
        get => mousedOver;
        set
        {
            mousedOver = value;
            OnMousedOver?.Invoke(this, value);
        }
    }
    public event Action<CardDisplayer, bool> OnMousedOver;

    public int cardLayer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateDisplay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateDisplay()
    {
        srImage.sprite = card.data.image;
        srFrame.sprite = card.data.frame;
        smFrame.sprite = card.data.frame;

        txtName.text = card.data.name;
        txtCost.text = $"{Utility.GetSymbolString(card.data.cost)}";
        txtRPS.text = $"{Utility.GetSymbolString(card.data.rps)}";
        txtPower.text = $"{Utility.GetSymbolString(card.data.power)}";

        //sorting layers
        srBack.sortingLayerName = sortingLayer;
        srFrame.sortingLayerName = sortingLayer;
        smFrame.frontSortingLayerID = SortingLayer.NameToID(sortingLayer);
        smFrame.backSortingLayerID = SortingLayer.NameToID(sortingLayer);
        srImage.sortingLayerName = sortingLayer;
        cvs.sortingLayerName = sortingLayer;


        //layers
        int baseLayer = cardLayer * 10;
        srBack.sortingOrder = baseLayer;
        srFrame.sortingOrder = baseLayer + 1;
        smFrame.backSortingOrder = baseLayer + 1;
        srImage.sortingOrder = baseLayer + 2;
        smFrame.frontSortingOrder = baseLayer + 2;
        cvs.sortingOrder = baseLayer + 3;

    }

    internal static CardDisplayer Find(Card card)
    {
        return FindObjectsByType<CardDisplayer>(FindObjectsSortMode.None).FirstOrDefault(cd => cd.card == card);
    }
}
