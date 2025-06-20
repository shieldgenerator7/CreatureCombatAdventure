using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class ImageDisplayer : MonoBehaviour
{
    public List<SpriteRenderer> srs;

    public int updateLayer(int baselayer, string sortingLayerName)
    {
        //sorting layers
        srs.ForEach(sr => sr.sortingLayerName = sortingLayerName);
        //sorting order
        for (int i = 0; i < srs.Count; i++)
        {
            SpriteRenderer sr = srs[i];
            sr.sortingOrder = baselayer + i;
        }
        return srs.Count;
    }
}
