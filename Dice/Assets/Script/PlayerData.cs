using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<RTDiceData> Dices;
}

public struct RTDiceData
{
    public List<RTSideData> Sides;

    public bool Used;

    public bool Rerolled;

    public int Index;

    public bool Rerollable;
    
    public (int sideIndex, RTSideData side) Roll()
    {
        return Sides.RandomlyGetOneWithIndex();
    }
}


public struct RTSideData
{
    public DiceSideEffect Side;
}