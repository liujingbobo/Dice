using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Heal : DiceSide
{
    public int value;
    
    public override IEnumerator TakeAction(Unit self, Unit target)
    {
        Debug.Log($"{self.ID} heal {value};");
        target.Heal(value);
        yield return null;
    }
}
