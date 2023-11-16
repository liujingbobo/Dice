using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public BuffManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    void Init(List<string> units)
    {
        unitBuffDic = new Dictionary<string, Dictionary<BuffType, Effect>>();

        foreach (var unit in units)
        {
            unitBuffDic[unit] = new Dictionary<BuffType, Effect>();
        }
    }

    private Dictionary<string, Dictionary<BuffType, Effect>> unitBuffDic;
    

    private Dictionary<BuffType, Effect> presetBuff;
    
    public IEnumerator AddBuff(BuffAction action)
    {
        if (!unitBuffDic.ContainsKey(action.Target))
        {
            unitBuffDic[action.Target] = new Dictionary<BuffType, Effect>();
        }

        Dictionary<BuffType, Effect> dic = unitBuffDic[action.Target];

        if (!dic.ContainsKey(action.BuffType))
        {
            Effect ef = Instantiate(presetBuff[action.BuffType]);
            
            dic[action.BuffType] = ef;
            
            B.AddEventTriggers(ef);
        }
        
        yield return presetBuff[action.BuffType].AddBuff(action);
    }

    public IEnumerator RemoveBuff(BuffAction action)
    {
        if (unitBuffDic.TryGetValue(action.Target, out var dic))
        {
            if (dic.TryGetValue(action.BuffType, out var b))
            {
                yield return b.RemoveBuff(action);
            }
        }
    }
}
