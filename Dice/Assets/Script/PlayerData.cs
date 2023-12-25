using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<BTDiceData> Dices;
}

public struct BTDiceData
{
    public List<BTSideData> Sides;

    public bool Used;

    public int RerollCount;

    public int Index;

    public bool Rerollable;

    public int RolledResult;
    
    public (int sideIndex, BTSideData side) Roll()
    {
        return Sides.RandomlyGetOneWithIndex();
    }

    public readonly BTSideData GetSide()
    {
        return !HasSide() ? default : Sides[RolledResult];
    }

    public readonly bool HasSide()
    {
        return RolledResult >= 0 && RolledResult < Sides.Count;
    }
}


public struct BTSideData
{
    public DiceSideEffect Side;
}