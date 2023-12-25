using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragTestChild : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerMoveHandler, IPointerClickHandler
{
    public string myName;
    public void OnDrag(PointerEventData eventData)
    {
        print($"{myName} Mouse {Input.mousePosition}");
        print($"{myName} Event {eventData.position}");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        print($"{myName} Drag Begin");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print($"{myName} Drag End");
    }

    public void OnPointerMove(PointerEventData eventData)
    {
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        print($"{myName} Pointer Click");

    }
}
