using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public Dictionary<BuffType, Effect> buffs;

    public IEnumerator Activate(BuffAction action)
    {
        yield return StartCoroutine(buffs[action.BuffType].Init(action));
    }
}
