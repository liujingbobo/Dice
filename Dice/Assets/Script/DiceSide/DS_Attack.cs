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
             SourceType = SourceType.Unit,
             Source = actionInfo.Source,
             Target = actionInfo.Target,
             Value = value[actionInfo.Level],
             IgnoreBarrier = false,
             SideEffectEffect = this
         };
         
         yield return Action(dmgInfo);
     }

     IEnumerator Action(DamageInfo info)
     {
         yield return BattleManager.Instance.DealDamage(info);
     }

     public class AttackInfo
     {
         public bool Canceled;
         public BTUnit From;
         public BTUnit Target;
         public int Value;
     }
}
