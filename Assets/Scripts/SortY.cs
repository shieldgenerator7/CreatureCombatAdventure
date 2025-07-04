using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SortY : MonoBehaviour
{
    public int offset = 0;
    public int offsetPerIndex = 0;
    //TODO: make it work with ImageDisplayer
    public List<SpriteRenderer> srs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (srs == null || srs.Count == 0)
        {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
            srs.Add(sr);
        }
        for(int i =0;  i < srs.Count; i++) {
            SpriteRenderer sr = srs[i];
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10) + offset + (offsetPerIndex*i);
        }
    }
}
