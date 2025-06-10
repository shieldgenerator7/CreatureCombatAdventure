using TMPro;
using UnityEngine;

public class CardDisplayer : MonoBehaviour
{
    public CreatureCard card;

    public SpriteRenderer srImage;
    public SpriteRenderer srFrame;
    public SpriteMask smFrame;

    public TMP_Text txtName;
    public TMP_Text txtCost;
    public TMP_Text txtRPS;
    public TMP_Text txtPower;

    public CardHolder holder;

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
        txtCost.text = $"{card.cost}";
        txtRPS.text = $"{card.rps}";
        txtPower.text = $"{card.power}";
    }
}
