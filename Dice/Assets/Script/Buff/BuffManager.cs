using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class BuffManager : SerializedMonoBehaviour
{
    [HideInInspector] public static BuffManager Instance;
    
    [SerializeField] private Dictionary<BuffType, Effect> presetBuff;
    
    private Dictionary<string, Dictionary<BuffType, Effect>> _unitBuffDic;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void Init(List<string> units)
    {
        _unitBuffDic = new Dictionary<string, Dictionary<BuffType, Effect>>();

        foreach (var unit in units)
        {
            _unitBuffDic[unit] = new Dictionary<BuffType, Effect>();
        }
    }

    public Effect GetBuffInfo(string unitID, BuffType buffType)
    {
        return _unitBuffDic[unitID][buffType];
    }

    
    public IEnumerator AddBuff(BuffAction action)
    {
        if (!_unitBuffDic.ContainsKey(action.Target))
        {
            _unitBuffDic[action.Target] = new Dictionary<BuffType, Effect>();
        }

        var dic = _unitBuffDic[action.Target];

        if (!dic.ContainsKey(action.BuffType))
        {
            var ef = Instantiate(presetBuff[action.BuffType]);
            
            dic[action.BuffType] = ef;
            
            B.AddEventTriggers(ef, action.Target);
        }
        
        yield return dic[action.BuffType].AddBuff(action);
    }

    public IEnumerator RemoveBuff(BuffAction action)
    {
        if (_unitBuffDic.TryGetValue(action.Target, out var dic))
        {
            if (dic.TryGetValue(action.BuffType, out var b))
            {
                yield return b.RemoveBuff(action);
            }
        }
    }

    public IEnumerator RemoveUnit(string id)
    {
        _unitBuffDic.Remove(id);
        yield break;    
    }
}
