using UnityEngine;

public struct MoveWeight
{
    public Move move;
    public int weight;

    public MoveWeight(Move move, int weight = 0)
    {
        this.move = move;
        this.weight = weight;
    }
}
