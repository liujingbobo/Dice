using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Attack : DiceSideEffect
{
     [FormerlySerializedAs("Value")] public int value;
     
     public override IEnumerator TakeAction(Unit self, Unit target)
     {
         AttackInfo info = new AttackInfo()
         {
             From = self,
             Target = target,
             Canceled = false,
             Value = value
         };
         
         BattleEvents.Instance.BeforeAttack.Invoke(info);

         yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.ProcessActions());

         if (info.Canceled)
         {
             Debug.Log("Attack is canceled.");
         }
         else
         {
             Debug.Log($"{self.ID} deal {value} damage to {target.ID}");

             yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.DealDamage(new DamageInfo()
             {
                 Source = self.ID,
                 Target = target.ID,
                 Value = info.Value,
                 IgnoreBarrier = false,
                 SideEffectEffect = this
             }));
         }
     }

     public class AttackInfo
     {
         public bool Canceled;
         public Unit From;
         public Unit Target;
         public int Value;
     }
}
