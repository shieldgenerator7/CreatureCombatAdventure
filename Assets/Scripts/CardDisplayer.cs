using TMPro;
using UnityEngine;

public class CardDisplayer : MonoBehaviour
{
    public CreatureCard card;

    public TMP_Text txtName;
    public TMP_Text txtCost;
    public TMP_Text txtRPS;
    public TMP_Text txtPower;

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
        txtName.text = card.name;
        txtCost.text = $"{card.cost}";
        txtRPS.text = $"{card.rps}";
        txtPower.text = $"{card.power}";
    }
}
