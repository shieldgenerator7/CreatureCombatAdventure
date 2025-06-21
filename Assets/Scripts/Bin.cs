using UnityEngine;

/// <summary>
/// Managers the folder for spawned objects. Usually used to clear spawned objects for a reset
/// </summary>
public class Bin : MonoBehaviour
{

    public void clearBin()
    {
        foreach (Transform t in transform)
        {
            if (t != null && t != transform)
            {
                Destroy(t.gameObject);
            }
        }
    }

    private static Bin instance;
    /// <summary>
    /// The game object used as a folder to store spawned objects
    /// </summary>
    public static Bin Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<Bin>();
            }
            return instance;
        }
    }
    public static Transform Transform=>Instance.transform;
}
