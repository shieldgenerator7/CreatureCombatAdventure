using UnityEngine;

public class SortY : MonoBehaviour
{
    public int offset = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y*10) + offset;
    }
}
