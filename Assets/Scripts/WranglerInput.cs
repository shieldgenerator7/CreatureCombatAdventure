using UnityEngine;

[DisallowMultipleComponent]
public abstract class WranglerInput : MonoBehaviour
{
    public WranglerController controller;

    public virtual void processTurn() { }
}
