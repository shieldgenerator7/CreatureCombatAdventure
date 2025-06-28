using System.Linq;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// Converts number to symbol string. Examples:
    /// 0 = O
    /// 1 = I
    /// 2 = Z
    /// 3 = ZI
    /// 4 = A
    /// 5 = AI
    /// 6 = AZ
    /// 7 = AZI
    /// 8 = B
    /// 9 = BI
    /// 10 = BZ
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="UnityException"></exception>
    public static string GetSymbolString(int number)
    {
        if (number < 0)
        {
            throw new UnityException($"Can't process a negative number! {number}");
        }

        //O I Z A B G E H K S
        string[] symbolList = { "I", "Z", "A", "B", "G", "E", "H", "K", "S" };
        if (number == 0)
        {
            return "O";
        }
        string str = "";
        while (number > 0)
        {
            for (int i = symbolList.Length - 1; i >= 0; i--)
            {
                int power = (int)Mathf.Pow(2, i);
                if (number >= power)
                {
                    str += symbolList[i];
                    number -= power;
                    break;
                }
            }
        }
        return str;
    }

    public static string GetSymbolString(RockPaperScissors rps)
    {
        switch (rps)
        {
            case RockPaperScissors.NONE:
                return "";
            case RockPaperScissors.ROCK:
                return "[R]";
            case RockPaperScissors.PAPER:
                return "|P|";
            case RockPaperScissors.SCISSORS:
                return "\\S/";
            default:
                throw new System.Exception($"Unknown rps symbol: {rps}");
        }
    }
}
