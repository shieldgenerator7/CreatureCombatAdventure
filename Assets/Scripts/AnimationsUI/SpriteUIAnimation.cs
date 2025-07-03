using Unity.VisualScripting;
using UnityEngine;

public class SpriteUIAnimation : UIAnimation
{
    private SpriteRenderer sr;
    private Sprite sprite;

    protected override void startAnimation()
    {
    }
    protected override void animate(float percent)
    {
        sr.sprite = sprite;
    }

    protected override void endAnimation()
    {
        sr.sprite = sprite;
    }

    public static SpriteUIAnimation ChangeSprite(SpriteRenderer sr, Sprite sprite)
    {
        SpriteUIAnimation suia = sr.AddComponent<SpriteUIAnimation>();
        suia.sr = sr;
        suia.sprite = sprite;

        UIAnimationQueue.Instance.queueAnimation(suia);
        return suia;
    }
}
