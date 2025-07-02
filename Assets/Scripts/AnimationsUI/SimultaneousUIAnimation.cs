using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimultaneousUIAnimation : UIAnimation
{
    private List<UIAnimation> _animations = new List<UIAnimation>();

    public override float Speed => _animations.Average(anim=>anim.Speed);

    protected override void startAnimation()
    {
        _animations.ForEach(anim=>anim.StartAnimation());
    }

    protected override void animate(float percent)
    {
        _animations.ForEach(anim => anim.animateProxy(percent));
    }

    protected override void endAnimation()
    {
        _animations.ForEach(anim => anim.EndAnimation());
    }

    public static SimultaneousUIAnimation AnimateTogether(params UIAnimation[] animations)
    {
        return AnimateTogether(new List<UIAnimation>(animations));
    }
    public static SimultaneousUIAnimation AnimateTogether(List<UIAnimation> animations)
    {
        GameObject go = FindAnyObjectByType<GameManager>().gameObject;
        SimultaneousUIAnimation suia = go.AddComponent<SimultaneousUIAnimation>();
        suia._animations.AddRange(animations);

        UIAnimationQueue.Instance.queueAnimation(suia);
        return suia;
    }
}
