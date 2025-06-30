using System.Collections.Generic;
using UnityEngine;

public class UIAnimationQueue : MonoBehaviour
{
    private List<UIAnimation> animationQueue = new List<UIAnimation>();
    private UIAnimation uiAnim;

    public static UIAnimationQueue Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void queueAnimation(UIAnimation animation)
    {
        animationQueue.Add(animation);
        if (!uiAnim)
        {
            processNextAnimation();
        }
    }
    private void processNextAnimation()
    {
        if (animationQueue.Count == 0) { return; }
        uiAnim = animationQueue[0];
        uiAnim.OnFinished += () =>
        {
            animationQueue.Remove(uiAnim);
            uiAnim = null;
            processNextAnimation();
        };
        uiAnim.StartAnimation();
    }

    public void Reset()
    {
        animationQueue.Clear();
        if (uiAnim != null)
        {
            uiAnim.enabled = false;
            uiAnim = null;
        }
    }
}
