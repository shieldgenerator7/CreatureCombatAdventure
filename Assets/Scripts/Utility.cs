using System.Linq;
using UnityEngine;

public static class Utility
{
    public static string GetSymbolString(int number)
    {
        if (number < 0)
        {
            throw new UnityException($"Can't process a negative number! {number}");
        }

        //O I Z A B G E H K S
        string[] symbolList = { "I", "Z", "A", "B", "G", "E", "H", "K", "S" };
        if (number ==0)
        {
            return "O";
        }
        string str = "";
        while (number > 0)
        {
            for(int i =symbolList.Length-1; i>= 0; i--)
            {
                int power = (int)Mathf.Pow(number,i);
                if (number >= power)
                {
                    str += symbolList[i];
                    number -= i;
                    break;
                }
            }
        }
        return str;
    }
}
