using UnityEngine;

[CreateAssetMenu(menuName ="Secret")]
public class SecretData:ScriptableObject
{
    public GameObject prefab;
    public Vector2 pos;
    public float scale = 1;
    public CreatureCardData reward;
}
