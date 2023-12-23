using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance;

    public Dictionary<Type, List<TriggerPack>> effectDic;

    public UnityEvent<BattleState> onSwitchingState = new UnityEvent<BattleState>();

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
        effectDic = new Dictionary<Type, List<TriggerPack>>();
    }

    public void AddTrigger(Effect effect, string owner)
    {
        foreach (var it in effect.GetType().GetInterfaces())
        {
            if (!effectDic.ContainsKey(it))
            {
                effectDic[it] = new List<TriggerPack>();
            }
            effectDic[it].Add(new TriggerPack()
            {
                Owner = owner,
                Trigger = (EffectTrigger)effect
            });
        }
    }

    public IEnumerator Do<T>(Func<T, IEnumerator> act)
    {
        if (!effectDic.TryGetValue(typeof(T), out var packs)) yield break;
        
        // Create a copy of the effects collection
        var packsCopy = new List<object>(packs.Select(_ => _.Trigger));

        foreach (var trigger in packsCopy.OfType<T>())
        {
            yield return act(trigger);
        }
    }

    public IEnumerator RemoveUnit(string id)
    {
        foreach (var kvp in effectDic)
        {
            kvp.Value.RemoveAll(pack => pack.Owner == id);
        }
        yield break;
    }
}

public struct TriggerPack
{
    public string Owner;
    public EffectTrigger Trigger;
}