using UnityEngine;

public class SecretDisplayer : MonoBehaviour
{
    private SecretData secret;

    public void init(SecretData secret)
    {
        this.secret = secret;
        this.transform.position = secret.pos;
        this.transform.localScale = Vector3.one * secret.scale;
    }

    public CreatureCardData found()
    {
        Destroy(this.gameObject);
        return secret.reward;
    }
}
