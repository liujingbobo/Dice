using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideEffect : ScriptableObject
{
    public Sprite Icon;
    [SerializeField] private SideType _sideType;
    [SerializeField] private bool _isPositive;
    public SideType SideType => _sideType;
    public bool isPositive => isPositive;

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


