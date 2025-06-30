using System;
using UnityEngine;

public abstract class UIAnimation : MonoBehaviour
{

    protected abstract void startAnimation();

    public float duration = 1;

    private float startTime = 0;

    private void Start()
    {
    }


    public void StartAnimation()
    {
        startTime = Time.time;
        startAnimation();
        enabled = true;
    }

    protected abstract void animate(float percent);

    public void EndAnimation()
    {
        enabled = false;
        endAnimation();
        finished();
        Destroy(this);
    }
    protected abstract void endAnimation();

    protected void finished()
    {
        OnFinished?.Invoke();
        OnFinished = null;
    }
    public event Action OnFinished;

    private void Update()
    {
        if (this.enabled && startTime > 0)
        {
            float percent = (Time.time - startTime) / duration;
            animate(percent);
            if (Time.time >= startTime + duration)
            {
                EndAnimation();
            }
        }
    }
}
