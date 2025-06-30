using UnityEngine;

public class MoveUIAnimation : UIAnimation
{
    private Vector3 startPos;
    private Vector3 startScale;
    private float startRotZ;
    private Transform target;

    public override float Speed => 1;

    protected override void startAnimation()
    {
        transform.position = startPos;
        transform.localScale = startScale;
        Vector3 rot2 = transform.eulerAngles;
        rot2.z = startRotZ;
        transform.eulerAngles = rot2;
    }

    protected override void endAnimation()
    {
        transform.position = target.position;
        transform.localScale = target.localScale;
        Vector3 rot2 = transform.eulerAngles;
        rot2.z = target.eulerAngles.z;
        transform.eulerAngles = rot2;
    }

    // Update is called once per frame
    protected override void animate(float percent)
    {
        percent = percent * percent;
        transform.position = Vector2.Lerp(startPos, target.position, percent);
        transform.localScale = Vector2.Lerp(startScale, target.localScale, percent);
        Vector3 rot = transform.eulerAngles;
        rot.z = Mathf.Lerp(startRotZ, target.eulerAngles.z, percent);
        transform.eulerAngles = rot;
    }

    public static MoveUIAnimation moveTo(GameObject go, Transform target)
    {
        MoveUIAnimation move = null;// go.GetComponent<MoveUIAnimation>();
        if (!move)
        {
            move = go.AddComponent<MoveUIAnimation>();
        }

        move.startPos = go.transform.position;
        move.startScale = go.transform.localScale;
        move.startRotZ = go.transform.eulerAngles.z;
        move.target = target;

        UIAnimationQueue.Instance.queueAnimation(move);
        return move;
    }
}
