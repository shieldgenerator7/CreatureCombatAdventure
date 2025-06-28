using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NumberUIAnimation : UIAnimation
{

    private int startNumber = 0;
    private int endNumber = 0;

    private float changeDuration = 1;
    private float delay;
    private float lastChangeTime= 0;

    private int number = 0;

    private SymbolSetData symbolSetData;

    TMP_Text txtNumber;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastChangeTime + delay)
        {
            lastChangeTime += delay;
            number += (int)Mathf.Sign(endNumber - startNumber);
            txtNumber.text = symbolSetData.GetSymbolString(number);
            if (number == endNumber)
            {
                finished();
            }
        }
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
        nuia.number = startNumber;
        nuia.enabled = true;
        nuia.delay = nuia.changeDuration / Mathf.Abs(endNumber - startNumber);
        nuia.lastChangeTime = Time.time;
        nuia.txtNumber.text = nuia.symbolSetData.GetSymbolString(nuia.number);
        return nuia;
    }
}
