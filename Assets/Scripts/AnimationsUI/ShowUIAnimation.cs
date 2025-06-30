using UnityEngine;

public class ShowUIAnimation : UIAnimation
{
    bool show;

    public override float Speed => 0;

    protected override void startAnimation()
    {
        EndAnimation();
    }
    protected override void animate(float percent)
    {
    }

    protected override void endAnimation()
    {
        gameObject.SetActive(show);
    }

    public static ShowUIAnimation showObject(GameObject go, bool show)
    {
        ShowUIAnimation showAnim = null;//go.GetComponent<ShowUIAnimation>();
        if (!showAnim)
        {
            showAnim = go.AddComponent<ShowUIAnimation>();
        }

        showAnim.show = show;

        UIAnimationQueue.Instance.queueAnimation(showAnim);
        return showAnim;
    }
}
