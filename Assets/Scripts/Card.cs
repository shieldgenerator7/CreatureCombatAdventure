using UnityEngine;

//[CreateAssetMenu(menuName ="Cards/")]
public abstract class Card:ScriptableObject
{
    public new string name;
    public int cost;
}
