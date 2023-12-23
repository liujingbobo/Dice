using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DiceSide", menuName = "DiceSide/Block", order = 1)]
public class DS_Block : DiceSideEffect
{
    [SerializeField] private int value;

    public override IEnumerator TakeAction(ActionInfo actionInfo)
    {
        var info = new GainBlockInfo(actionInfo.Source, actionInfo.Target, value);
        
        yield return Action(info);
    }
    
    IEnumerator Action(GainBlockInfo info)
    {
        yield return BattleManager.Instance.GainBlock(info);
    }
}
