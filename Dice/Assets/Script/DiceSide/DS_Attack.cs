using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Attack : DiceSide
{
     [FormerlySerializedAs("Value")] public int value;
     
     public override IEnumerator TakeAction(Unit self, Unit target)
     {
         AttackInfo info = new AttackInfo(self, target);
         
         BattleEvents.Instance.BeforeAttack.Invoke(info);
         
         Debug.Log($"{self.ID} deal {value} damage to {target.ID}");
         
         target.DealDamage(value);
         
         yield return null;
     }

     public class AttackInfo
     {
         public bool Canceled;
         public Unit From;
         public Unit Target;

         public AttackInfo(Unit from, Unit Target)
         {
             
         }
     }
}
