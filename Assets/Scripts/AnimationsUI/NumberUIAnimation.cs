using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NumberUIAnimation : UIAnimation
{

    private int startNumber = 0;
    private int endNumber = 0;

    private float diff;

    private int number = 0;

    private SymbolSetData symbolSetData;

    TMP_Text txtNumber;


    public int Number
    {
        get => number;
        set
        {
            number = value;
            txtNumber.text = symbolSetData.GetSymbolString(number);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    protected override void startAnimation()
    {
        Number = startNumber;
    }

    // Update is called once per frame
    protected override void animate(float percent)
    {
        Number = Mathf.RoundToInt(percent * diff + startNumber);
    }
    protected override void endAnimation()
    {
            Number = endNumber;
    }

    public static NumberUIAnimation adjustTo(TMP_Text text, int startNumber, int endNumber, SymbolSetData ssd)
    {
        NumberUIAnimation nuia = text.GetComponent<NumberUIAnimation>();
        if (!nuia)
        {
            nuia= text.AddComponent<NumberUIAnimation>();
        }
        nuia.txtNumber = text;
        nuia.startNumber = startNumber;
        nuia.endNumber = endNumber;
        nuia.symbolSetData = ssd;
        nuia.diff = Mathf.Abs(endNumber - startNumber);

        UIAnimationQueue.Instance.queueAnimation(nuia);
        return nuia;
    }
}
