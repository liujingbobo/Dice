using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiceDragIndicator : MonoBehaviour
{

    [SerializeField] private GameObject sideFollower;

    public void StartDrag()
    {
        sideFollower.SetActive(true);
    }

    public void EndDrag()
    {
        sideFollower.SetActive(false);
    }
}
