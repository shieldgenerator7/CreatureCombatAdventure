using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NumberUIAnimation : UIAnimation
{

    private int startNumber = 0;
    private int endNumber = 0;

    private float diff;

    private int number = 0;

    private Color color;

    private SymbolSetData symbolSetData;
    private Func<int, string> formatFunc;

    TMP_Text txtNumber;

    public override float Speed => 0.5f;


    public int Number
    {
        get => number;
        set
        {
            number = value;
            if (formatFunc != null)
            {
                txtNumber.text = formatFunc(number);
            }
            else
            {
                txtNumber.text = symbolSetData.GetSymbolString(number);
            }
            txtNumber.color = color;
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

    public static NumberUIAnimation adjustTo(TMP_Text text, int startNumber, int endNumber, Color color, SymbolSetData ssd, Func<int, string> formatFunc = null)
    {
        NumberUIAnimation nuia = null;// text.GetComponent<NumberUIAnimation>();
        if (!nuia)
        {
            nuia = text.AddComponent<NumberUIAnimation>();
        }

        nuia.txtNumber = text;
        nuia.startNumber = startNumber;
        nuia.endNumber = endNumber;
        nuia.color = color;
        nuia.symbolSetData = ssd;
        nuia.formatFunc = formatFunc;
        nuia.diff = endNumber - startNumber;

        UIAnimationQueue.Instance.queueAnimation(nuia);
        return nuia;
    }
}
