using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SymbolSetData")]
public class SymbolSetData : ScriptableObject
{
    [Header("Power")]
    public List<string> powerSymbols;
    public enum PowerSymbolSystem
    {
        DECIMAL,//O, A, B, C, D, E, F, G, H, I, AO
        BINARY,//O, IO, II, IOO, IOI, IIO, III, IOOO, IOOI, IOIO
        BINARY_STACKED,//O, I, Z, ZI, A, AI, AZ, AZI, B, BI, BZ
    }
    public PowerSymbolSystem powerSymbolSystem = PowerSymbolSystem.BINARY_STACKED;

    //TODO: make this work with modular RPS values
    [Header("RPS")]
    public string symbolRPSNone;
    public string symbolRPSRock;
    public string symbolRPSPaper;
    public string symbolRPSScissors;

    /// <summary>
    /// Converts number to symbol string.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    /// <exception cref="UnityException"></exception>
    public string GetSymbolString(int number)
    {
        if (number < 0)
        {
            throw new UnityException($"Can't process a negative number! {number}");
        }

        switch (powerSymbolSystem)
        {
            case PowerSymbolSystem.DECIMAL:
                if (powerSymbols.Count != 10)
                {
                    throw new UnityException("For DECIMAL, there needs to be 10 symbols! 1 for \"0\", and then 9 for the other digits, in sequential order.");
                }
                if (number == 0)
                {
                    return powerSymbols[0];
                }
                string strdec = "";
                int digits = Mathf.CeilToInt(Mathf.Log10(number));
                if ((int)Mathf.Pow(10, digits) == number)
                {
                    digits++;
                }
                for (int i = digits; i > 0; i--)
                {
                    float ten = Mathf.Pow(10, i - 1);
                    int digit = (int)(Mathf.Floor(number / ten) % 10);
                    strdec += powerSymbols[digit];
                }
                return strdec;
            case PowerSymbolSystem.BINARY_STACKED:
                if (number > Mathf.Pow(2, (powerSymbols.Count - 1) - 1))
                {
                    throw new UnityException($"BINARY_STACKED Symbol set doesnt have enough symbols to accomodate value! {number}");
                }
                if (number == 0)
                {
                    return powerSymbols[0];
                }
                string str = "";
                while (number > 0)
                {
                    for (int i = powerSymbols.Count - 1; i >= 1; i--)
                    {
                        int power = (int)Mathf.Pow(2, i - 1);
                        if (number >= power)
                        {
                            str += powerSymbols[i];
                            number -= power;
                            break;
                        }
                    }
                }
                return str;
            default:
                throw new NotImplementedException();
        }
    }

    public string GetSymbolString(RockPaperScissors rps)
    {
        switch (rps)
        {
            case RockPaperScissors.NONE:
                return symbolRPSNone;
            case RockPaperScissors.ROCK:
                return symbolRPSRock;
            case RockPaperScissors.PAPER:
                return symbolRPSPaper;
            case RockPaperScissors.SCISSORS:
                return symbolRPSScissors;
            default:
                throw new System.Exception($"Unknown rps symbol: {rps}");
        }
    }
}
