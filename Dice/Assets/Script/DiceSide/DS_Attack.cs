using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSide", menuName = "DiceSide/Attack", order = 0)]
public class DS_Attack : DiceSideEffect
{
     public List<int> value;
     public override int MaxLevel => value.Count - 1;
     public override IEnumerator TakeAction(ActionInfo actionInfo)
     {
         DamageInfo dmgInfo = new DamageInfo()
         {
             Source = actionInfo.Source.ID,
             Target = actionInfo.Target.ID,
             Value = value[actionInfo.Level],
             IgnoreBarrier = false,
             SideEffectEffect = this
         };
         
         yield return BattleManager.Instance.StartCoroutine(Action(dmgInfo));
     }

     IEnumerator Action(DamageInfo info)
     {
         // Play Animation
         yield return new WaitForSeconds(1f);
         
         yield return BattleManager.Instance.StartCoroutine(BattleManager.Instance.DealDamage(info));
     }

     public class AttackInfo
     {
         public bool Canceled;
         public Unit From;
         public Unit Target;
         public int Value;
     }
}
