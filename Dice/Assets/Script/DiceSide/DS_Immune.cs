using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImmuneSide", menuName = "DiceSide/Immune", order = 0)]
public class DS_Immune : DiceSideEffect
{
    public override IEnumerator TakeAction(Unit self, Unit target)
    {
        BattleEvents.Instance.BeforeAttack.AddListener(_ =>
        {
            
        });
        return base.TakeAction(self, target);
    }
}
