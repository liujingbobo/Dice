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
}


public class RTSideData
{
    public DiceSideEffect Side;
    public int Level;
}