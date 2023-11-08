using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSide", menuName = "DiceSide/Block", order = 1)]
public class DS_Block : DiceSide
{
    public int Value;
    
    public override IEnumerator TakeAction(Unit self, Unit target)
    {
        target.DealDamage(Value);
        yield return null;
    }
}
