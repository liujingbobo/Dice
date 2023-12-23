using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Buff : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text stack;

    public void Refresh(BTUnit unit, BuffType buffType, Effect info)
    {
        if(icon) icon.sprite = info.icon;
        
        if(stack) stack.text = info.byStack ? unit.buffs[buffType].ToString() : string.Empty;
    }
}
