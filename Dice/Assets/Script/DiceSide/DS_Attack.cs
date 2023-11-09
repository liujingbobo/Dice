using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Attack : DiceSide
{
     public int Value;
     
     public override IEnumerator TakeAction(Unit self, Unit target)
     {
         Debug.Log($"{self.ID} deal {Value} damage to {target.ID}");
         target.DealDamage(Value);
         yield return null;
     }
}
