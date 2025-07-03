using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageDisplayer : MonoBehaviour
{
    public List<SpriteRenderer> srs;
    private List<SpriteMask> sms;

    public int updateLayer(int baselayer, string sortingLayerName)
    {
        if (sms == null || sms.Count != srs.Count)
        {
            initSpriteMaskList();
        }

        //sorting layers
        srs.ForEach(sr => sr.sortingLayerName = sortingLayerName);
        int sortingLayerID = SortingLayer.NameToID(sortingLayerName);
        sms.ForEach(sm =>
        {
            if (sm)
            {
                sm.frontSortingLayerID = sortingLayerID;
                sm.backSortingLayerID = sortingLayerID;
            }
        });
        //sorting order
        for (int i = 0; i < srs.Count; i++)
        {
            SpriteRenderer sr = srs[i];
            sr.sortingOrder = baselayer + i;

            SpriteMask sm = sms[i];
            if (sm)
            {
                sm.backSortingOrder = baselayer + i;
                sm.frontSortingOrder = baselayer + i + 1;
            }
        }
        return srs.Count;
    }

    internal void updateColors(List<Color> colors)
    {
        for (int i = 0; i < colors.Count; i++)
        {
            if (i >= srs.Count) { break; }

            SpriteRenderer sr = srs[i];
            sr.color = colors[i];
        }
    }

    private void initSpriteMaskList()
    {
        sms = srs.ConvertAll(sr => sr.GetComponent<SpriteMask>());
    }
}
