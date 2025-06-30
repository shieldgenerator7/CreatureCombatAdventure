using UnityEngine;

public class MoveUIAnimation : UIAnimation
{
    public float moveSpeed = 3f;
    public float threshold = 0.2f;
    private Transform target;

    protected override void startAnimation()
    {
    }

    protected override void endAnimation(){}

    // Update is called once per frame
    protected override void animate(float percent)
    {
        transform.position = Vector2.Lerp(transform.position, target.position, moveSpeed*Time.deltaTime);
        transform.localScale = Vector2.Lerp(transform.localScale, target.localScale, moveSpeed * Time.deltaTime);
        Vector3 rot = transform.eulerAngles;
        rot.z = Mathf.Lerp(rot.z, target.eulerAngles.z, moveSpeed * Time.deltaTime);
        transform.eulerAngles = rot;
        if (Vector2.Distance(transform.position, target.position) <= threshold)
        {
            transform.position = target.position;
            transform.localScale = target.localScale;
            Vector3 rot2 = transform.eulerAngles;
            rot2.z = target.eulerAngles.z;
            transform.eulerAngles = rot2;
            finished();
        }
    }

    public static MoveUIAnimation moveTo(GameObject go, Transform target)
    {
        MoveUIAnimation move = go.GetComponent<MoveUIAnimation>();
        if (!move)
        {
            move = go.AddComponent<MoveUIAnimation>();
        }
        move.target = target;
        UIAnimationQueue.Instance.queueAnimation(move);
        return move;
    }
}
