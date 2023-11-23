using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTest : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameObject Target;
    public GameObject cur; 
    public void OnDrag(PointerEventData eventData)
    {
        cur.transform.position = Input.mousePosition;
        print("OnDrag");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cur = Instantiate(Target, transform);
        cur.transform.position = this.transform.position;
        print("OnBeginDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        print("OnEndDrag");
    }
}
