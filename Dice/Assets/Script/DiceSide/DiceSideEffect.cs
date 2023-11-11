using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideEffect : ScriptableObject
{
    public virtual IEnumerator TakeAction(Unit self, Unit target)
    {
        yield return null;
    }
}
