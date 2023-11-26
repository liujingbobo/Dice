using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_Dice : MonoBehaviour
{
    private RTDiceData target;

    private int index;

    private bool rolled;

    [SerializeField] private Button rerollButton;

    [SerializeField] private GameObject resultSide;

    [SerializeField] private CacheLayoutPattern cache;

    [SerializeField] private int randomDisplayTime = 5;
    
    [SerializeField] private float randomDisplayGap = 0.5f;

    public void Init(RTDiceData data, int targetIndex)
    {
        target = data;
        index = targetIndex;
        
        foreach (var p in cache.Cache.Use(data.Sides))
        {
            if (p.Key.TryGetComponent<UI_Side>(out var side))
            {
                side.Init(p.Value);
            }
        }
        
        // Disable Reroll
    }
    
    public void SetRefreshButton(bool active)
    {
        if(rerollButton) rerollButton.gameObject.SetActive(active);    
    }
    
    public IEnumerator MockRoll(int result)
    {
        resultSide.SetActive(false);
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
