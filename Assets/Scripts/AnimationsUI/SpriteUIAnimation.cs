using Unity.VisualScripting;
using UnityEngine;

public class SpriteUIAnimation : UIAnimation
{
    private SpriteRenderer sr;
    private Sprite sprite;
    private Color color;

    protected override void startAnimation()
    {
    }
    protected override void animate(float percent)
    {
        sr.sprite = sprite;
        sr.color = color;
    }

    protected override void endAnimation()
    {
        sr.sprite = sprite;
        sr.color = color;
    }

    public static SpriteUIAnimation ChangeSprite(SpriteRenderer sr, Sprite sprite, Color color)
    {
        SpriteUIAnimation suia = sr.AddComponent<SpriteUIAnimation>();
        suia.sr = sr;
        suia.sprite = sprite;
        suia.color = color;

        UIAnimationQueue.Instance.queueAnimation(suia);
        return suia;
    }
}
