using System;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSetData:ScriptableObject
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
}
