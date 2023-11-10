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
        if (BattleManager.Instance.Units.ContainsKey(target))
        {
            BattleManager.Instance.Units[target].Subscribe(UpdateInfo).AddTo(this);
        }
    }

    void UpdateInfo(Unit unit)
    {
        HP.text = unit.HP.ToString();
        BR.text = unit.BR.ToString();
    }

}
