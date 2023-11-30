using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public List<RTDiceData> Dices;
}

public class RTDiceData
{
    public List<RTSideData> Sides;

    public int testValue;
    
    public (int sideIndex, RTSideData side) Roll()
    {
        return Sides.RandomlyGetOneWithIndex();
    }
}


public class RTSideData
{
    public DiceSideEffect Side;
    public int Level;
}