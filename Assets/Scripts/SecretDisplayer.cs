using UnityEngine;

public class SecretDisplayer : MonoBehaviour
{
    private SecretData secret;

    public void init(SecretData secret)
    {
        this.secret = secret;
    }

    public CreatureCardData found()
    {
        Destroy(this.gameObject);
        return secret.reward;
    }
}
