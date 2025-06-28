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
    public SpriteRenderer srHighlight;

    public Canvas cvs;
    public TMP_Text txtName;
    public TMP_Text txtCost;
    public TMP_Text txtRPS;
    public TMP_Text txtPower;

    private ImageDisplayer imageDisplayer;

    public string sortingLayer = "Card";

    private bool mousedOver = false;
    public bool MousedOver
    {
        get => mousedOver;
        set
        {
            mousedOver = value;
            acceptHover(mousedOver);
            OnMousedOver?.Invoke(this, value);
        }
    }
    public event Action<CardDisplayer, bool> OnMousedOver;

    public int cardLayer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateDisplay();
        acceptHover(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateDisplay()
    {
        if (srImage)
        {
            srImage.sprite = card.data.image;
            if (card.data.colors.Count > 0)
            {
                srImage.color = card.data.colors.First();
            }
        }
        srFrame.sprite = card.data.frame;
        srFrame.color = card.data.frameColor;
        smFrame.sprite = card.data.frame;

        txtName.text = card.data.name;
        //txtCost.text = $"{Utility.GetSymbolString(card.data.cost)}";
        txtRPS.text = $"{GameManager.SymbolSetData.GetSymbolString(card.data.rps)}";
        txtPower.text = $"{GameManager.SymbolSetData.GetSymbolString(card.data.power)}";

        //sorting layers
        srBack.sortingLayerName = sortingLayer;
        srFrame.sortingLayerName = sortingLayer;
        smFrame.frontSortingLayerID = SortingLayer.NameToID(sortingLayer);
        smFrame.backSortingLayerID = SortingLayer.NameToID(sortingLayer);
        if (srImage)
        {
            srImage.sortingLayerName = sortingLayer;
        }
        cvs.sortingLayerName = sortingLayer;
        srHighlight.sortingLayerName = sortingLayer;


        //layers
        int baseLayer = cardLayer * 10;
        srHighlight.sortingOrder = baseLayer - 1;
        srBack.sortingOrder = baseLayer;
        srFrame.sortingOrder = baseLayer + 1;
        smFrame.backSortingOrder = baseLayer + 1;
        if (srImage)
        {
            srImage.sortingOrder = baseLayer + 2;
        }
        smFrame.frontSortingOrder = baseLayer + 2;
        int layeroffset = 0;

        if (card.data.imagePrefab)
        {
            if (!imageDisplayer)
            {
                setupImageDisplayer();
            }
            layeroffset = imageDisplayer.updateLayer(baseLayer + 2, sortingLayer);
        }
        cvs.sortingOrder = baseLayer + 3 + layeroffset;

    }

    public void acceptHover(bool hover)
    {
        srHighlight.enabled = hover;
    }

    private void setupImageDisplayer()
    {
        GameObject imageObject = Instantiate(card.data.imagePrefab, transform);
        imageObject.transform.localPosition = srImage.transform.localPosition;
        imageDisplayer = imageObject.GetComponent<ImageDisplayer>();

        imageDisplayer.updateColors(card.data.colors);
    }

    internal static CardDisplayer Find(Card card)
    {
        return FindObjectsByType<CardDisplayer>(FindObjectsSortMode.None).FirstOrDefault(cd => cd.card == card);
    }
}
