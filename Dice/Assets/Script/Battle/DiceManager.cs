using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance;
    
    public CacheLayoutPattern cache;

    public bool dragging;
    public RTDiceData CurrentDragging;

    [SerializeField] private float rollGap;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void Init(List<RTDiceData> dices)
    {
        var triplets = cache.Cache.UseByIndex(dices);
        
        foreach (var triplet in triplets)
        {
            if (triplet.Key.TryGetComponent<UI_BattleDice>(out var dice))
            {
                dice.Init(triplet.Value1);
            }
        }
    }
    
    public IEnumerator Roll(List<RTDiceData> infos)
    {
        var pairs = cache.Cache.UsingByIndex().ToList();
        for (int i = 0; i < infos.Count; i++)
        {
            var info = infos[i];
            
            if (pairs[info.Index].Key.TryGetComponent<UI_BattleDice>(out var dice))
            {
                if (i == infos.Count - 1)
                {
                    yield return dice.MockRoll(info.RolledResult);
                }
                else
                {
                    StartCoroutine(dice.MockRoll(info.RolledResult));
                    yield return rollGap;
                }
            }
        }
    }

    public void RefreshAll(List<RTDiceData> datas)
    {
        var pairs = cache.Cache.UsingByIndex().PairWith(datas);
        foreach (var pair in pairs)
        {
            if (pair.Item1.Key.TryGetComponent<UI_BattleDice>(out var dice))
            {
                dice.Refresh(pair.Item2);
            }
        }
    }
}
