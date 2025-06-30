using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NumberUIAnimation : UIAnimation
{

    private int startNumber = 0;
    private int endNumber = 0;

    private float changeDuration = 1;
    private float diff;
    private float lastChangeTime= 0;

    private int number = 0;

    private SymbolSetData symbolSetData;

    TMP_Text txtNumber;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void startAnimation()
    {
        txtNumber.text = symbolSetData.GetSymbolString(number);
        enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float percent = (Time.time - lastChangeTime) / changeDuration;
        number = Mathf.RoundToInt(percent * diff + startNumber);
        txtNumber.text = symbolSetData.GetSymbolString(number);
        if (Time.time >= lastChangeTime + changeDuration)
        {
            number = endNumber;
            txtNumber.text = symbolSetData.GetSymbolString(number);
                finished();
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
        nuia.diff = Mathf.Abs(endNumber - startNumber);
        nuia.lastChangeTime = Time.time;
        nuia.txtNumber.text = nuia.symbolSetData.GetSymbolString(nuia.number);

        UIAnimationQueue.Instance.queueAnimation(nuia);
        return nuia;
    }
}
