using UnityEngine;

public class MoveUIAnimation : UIAnimation
{
    private Transform start;
    private Transform target;

    protected override void startAnimation()
    {
        transform.position = start.position;
        transform.localScale = start.localScale;
        Vector3 rot2 = transform.eulerAngles;
        rot2.z = start.eulerAngles.z;
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
        transform.position = Vector2.Lerp(start.position, target.position, percent);
        transform.localScale = Vector2.Lerp(start.localScale, target.localScale, percent);
        Vector3 rot = start.eulerAngles;
        rot.z = Mathf.Lerp(rot.z, target.eulerAngles.z, percent);
        start.eulerAngles = rot;
    }

    public static MoveUIAnimation moveTo(GameObject go, Transform target)
    {
        MoveUIAnimation move = go.GetComponent<MoveUIAnimation>();
        if (!move)
        {
            move = go.AddComponent<MoveUIAnimation>();
        }
        move.start = go.transform;
        move.target = target;
        UIAnimationQueue.Instance.queueAnimation(move);
        return move;
    }
}
