using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BattleEvents : MonoBehaviour
{
    public static BattleEvents Instance;

    public Dictionary<Type, List<EffectTrigger>> EffectDic;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);            
        }
        Instance = this;
    }

    public void Init()
    {
        EffectDic = new Dictionary<Type, List<EffectTrigger>>();
    }

    public IEnumerator DO<T>(Func<T, IEnumerator> act)
    {
        if(EffectDic.TryGetValue(typeof(T), out var effects))
        {
            foreach (var trigger in effects)
            {
                if (trigger is T)
                {
                    yield return act((T)trigger);
                }
            }
        }
    }
}
