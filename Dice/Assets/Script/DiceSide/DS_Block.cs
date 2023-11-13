using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiceSide", menuName = "DiceSide/Block", order = 1)]
public class DS_Block : DiceSideEffect
{
    public int Value;
    
    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {

        BattleManager.Instance.StartCoroutine(BattleManager.Instance.GainBlock(new GainBlockInfo()
            {
                Value = Value,
                Source = actionInfo.Source.ID,
                Target = actionInfo.Source.ID
            }));
        
        yield return null;
    }
}
