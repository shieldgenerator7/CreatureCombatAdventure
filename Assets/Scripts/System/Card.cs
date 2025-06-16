using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class Card
{
    //TODO: allow other card types?
    public CreatureCardData data;
    [NonSerialized]
    public Wrangler owner;
    [NonSerialized]
    public CardHolder holder;

    public static implicit operator bool(Card card) => card != null;
}
