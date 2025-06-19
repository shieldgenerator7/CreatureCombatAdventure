using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Wrangler
{
    public string name;
    public List<Card> cardList;
    [NonSerialized]
    public List<CardHolder> cardHolders;
    [NonSerialized]
    public CardHolder handHolder;

    public static implicit operator bool(Wrangler wrangler) => wrangler != null;

}
