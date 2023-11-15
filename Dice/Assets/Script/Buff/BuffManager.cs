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

    void Init()
    {
        presetBuff = new Dictionary<BuffType, Effect>();
    }

    private Dictionary<BuffType, Effect> presetBuff;

    public IEnumerator Activate(BuffAction action)
    {
        yield return StartCoroutine(presetBuff[action.BuffType].Init(action));
    }
}
