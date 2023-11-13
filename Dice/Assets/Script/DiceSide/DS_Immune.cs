using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ImmuneSide", menuName = "DiceSide/Immune", order = 0)]
public class DS_Immune : DiceSideEffect
{
    public override IEnumerator TakeAction(ActionInfo info)
    {
        BattleEvents.Instance.BeforeAttack.AddListener(_ =>
        {
            
        });
        yield break;
    }
}
