using System;
using UnityEngine;

public abstract class UIAnimation : MonoBehaviour
{

    public abstract void startAnimation();

    protected void finished()
    {
        this.enabled = false;
        OnFinished?.Invoke();
        OnFinished = null;
    }
    public event Action OnFinished;
}
