using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIView_UnitStatus : MonoBehaviour
{
    public bool isPlayer;

    public Unit Target;
    public TMP_Text HP;
    public TMP_Text BR;

    public float popUpGap = 0.2f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Target = isPlayer ? BattleManager.Instance.Player : BattleManager.Instance.Enemy;

        // if (Target != null)
        // {
        //
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Target == null) return;
        HP.text = Target.HP.ToString();
        BR.text = Target.BR.ToString();
    }
}
