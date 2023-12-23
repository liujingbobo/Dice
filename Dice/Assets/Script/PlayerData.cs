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

    public int RerollCount;

    public int Index;

    public bool Rerollable;

    public int RolledResult;
    
    public (int sideIndex, RTSideData side) Roll()
    {
        return Sides.RandomlyGetOneWithIndex();
    }

    public readonly RTSideData GetSide()
    {
        return !HasSide() ? default : Sides[RolledResult];
    }

    public readonly bool HasSide()
    {
        return RolledResult >= 0 && RolledResult < Sides.Count;
    }
}


public struct RTSideData
{
    public DiceSideEffect Side;
}