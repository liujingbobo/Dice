using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Dice : MonoBehaviour
{
    private RTDiceData target;

    private int Index => target.Index;

    [SerializeField] private Button castButton;

    [SerializeField] private Button rerollButton;

    [SerializeField] private GameObject resultSide;

    [SerializeField] private CacheLayoutPattern cache;

    [SerializeField] private int randomDisplayTime = 5;
    
    [SerializeField] private float randomDisplayGap = 0.5f;

    public void Awake()
    {
        castButton.onClick.AddListener(Cast);
        rerollButton.onClick.AddListener(() => BattleManager.Instance.Reroll(Index));
    }

    public void Init(RTDiceData data)
    {
        target = data;
        
        var sides = cache.Cache.Use(data.Sides).ToList();
        
        foreach (var p in sides)
        {
            if (p.Key.TryGetComponent<UI_Side>(out var side))
            {
                side.Init(p.Value);
            }
        }
    }

    public void Refresh(RTDiceData data)
    {
        target = data;
        rerollButton.gameObject.SetActive(data.Rerollable);
    }

    public void Cast()
    {
        if (target.Used)
        {
            BattleManager.Instance.Cast(Index);
        }
    }

    public IEnumerator MockRoll(int result)
    {
        var allSide = cache.Cache.UsingByIndex().ToList();
        int left = randomDisplayTime;
        while (left > 0)
        {
            int ran = Random.Range(0, allSide.Count);
            allSide.ForEach(_ =>
            {
                if (_.Key.TryGetComponent<UI_Side>(out var side))
                {
                    side.SetHighLight(_.Value == ran);
                }
            });
            yield return new WaitForSeconds(randomDisplayGap);
            left--;
        }
        allSide.ForEach(_ =>
        {
            if (_.Key.TryGetComponent<UI_Side>(out var side))
            {
                side.SetHighLight(_.Value == result);
            }
        });
        yield return new WaitForSeconds(randomDisplayGap);
        if (allSide[result].Key.TryGetComponent<UI_Side>(out var side))
        {
            side.SetSelected(true);
        }
        
        // Refresh Reroll
    }
}
