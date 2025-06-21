using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wrangler
{
    public string name;
    public List<Card> cardList = new List<Card>();
    [NonSerialized]
    public List<CardHolder> cardHolders = new List<CardHolder>();
    [NonSerialized]
    public CardHolder handHolder;

    public static implicit operator bool(Wrangler wrangler) => wrangler != null;

}
