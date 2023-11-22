using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_Side : MonoBehaviour
{
    [SerializeField] private Image icon;
    
    [SerializeField] private CanvasGroup highLight;
    
    [SerializeField] private GameObject selectableFrame; // This will be activate when it's actually selected

    private DiceSideEffect _target;
    
    public void Init(DiceSideEffect content)
    {
        _target = content;
        if (icon) icon.sprite = _target.Icon;
    }

    public void Reset()
    {
        SetHighLight(false);
        SetSelected(true);
    }

    public void SetHighLight(bool s)
    {
        if (highLight) highLight.alpha = s ? 1 : 0.5f;
    }

    public void SetSelected(bool s)
    {
        if (highLight) highLight.alpha = s ? 1 : 0.5f;
        if(selectableFrame) selectableFrame.SetActive(s);
    }
}
