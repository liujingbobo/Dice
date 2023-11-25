using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIView_UnitStatus : MonoBehaviour
{
    public string target;
    
    public TMP_Text HP;
    public TMP_Text BR;

    public float popUpGap = 0.2f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (BattleManager.Instance.Units.TryGetValue(target, out var unit))
        {
            unit.Subscribe(UpdateInfo).AddTo(this);
        }
    }

    void UpdateInfo(BTUnit btUnit)
    {
        HP.text = btUnit.HP.ToString();
        BR.text = btUnit.BR.ToString();
    }
}
