using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView_Side : MonoBehaviour
{
    [SerializeField] private Image icon;

    [SerializeField] private CanvasGroup highLight;
    
    [SerializeField] private GameObject selectableFrame; // This will be activate when it's actually selected
    
    public void Fill(DiceSideEffect side)
    {
        if (icon) icon.sprite = side.icon;
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
