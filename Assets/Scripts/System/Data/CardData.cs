using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(menuName ="Cards/")]
public abstract class CardData : ScriptableObject
{
    public new string name;
    public int cost;

    public Sprite image;
    public Sprite frame;
    public Color frameColor = Color.white;
    [Tooltip("Optional replacement for image")]
    public GameObject imagePrefab;
    public List<Color> colors;
}
