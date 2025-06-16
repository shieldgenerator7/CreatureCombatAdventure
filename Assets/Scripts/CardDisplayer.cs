using TMPro;
using UnityEngine;

public class CardDisplayer : MonoBehaviour
{
    public CreatureCard card;

    public WranglerController owner;

    public SpriteRenderer srBack;
    public SpriteRenderer srImage;
    public SpriteRenderer srFrame;
    public SpriteMask smFrame;

    public Canvas cvs;
    public TMP_Text txtName;
    public TMP_Text txtCost;
    public TMP_Text txtRPS;
    public TMP_Text txtPower;

    public CardHolderDisplayer holder;

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
        srImage.sprite = card.image;
        srFrame.sprite = card.frame;
        smFrame.sprite = card.frame;

        txtName.text = card.name;
        txtCost.text = $"{Utility.GetSymbolString(card.cost)}";
        txtRPS.text = $"{Utility.GetSymbolString(card.rps)}";
        txtPower.text = $"{Utility.GetSymbolString(card.power)}";

        //layers
        int baseLayer = cardLayer * 10;
        srBack.sortingOrder = baseLayer;
        srFrame.sortingOrder = baseLayer + 1;
        smFrame.backSortingOrder = baseLayer + 1;
        srImage.sortingOrder = baseLayer + 2;
        smFrame.frontSortingOrder = baseLayer + 2;
        cvs.sortingOrder = baseLayer + 3;

    }
}
