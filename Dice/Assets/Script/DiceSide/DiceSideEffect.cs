using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class DiceSideEffect : SerializedScriptableObject
{
    public Sprite icon;
    [SerializeField] private SideType sideType;
    [SerializeField] private bool isPositive;
    [SerializeField] private TargetType targetType;
    public SideType SideType => sideType;
    public TargetType TargetType => targetType;
    
    public virtual IEnumerator TakeAction(ActionInfo actionInfo)
    {
        yield return null;
    }
}


