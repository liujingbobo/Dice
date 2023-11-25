using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DiceSideEffect : ScriptableObject
{
    public Sprite Icon;
    [SerializeField] private SideType sideType;
    [SerializeField] private bool isPositive;
    [SerializeField] private TargetType targetType;
    public SideType SideType => sideType;
    public bool IsPositive => isPositive;

    public virtual bool CanLevelUp(int curLevel)
    {
        return curLevel < MaxLevel;
    }
    
    public virtual int MaxLevel => 0;
    
    public virtual IEnumerator TakeAction(ActionInfo actionInfo)
    {
        yield return null;
    }
}


