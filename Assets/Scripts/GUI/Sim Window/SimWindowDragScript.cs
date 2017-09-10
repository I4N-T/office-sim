using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SimWindowDragScript : MonoBehaviour, IBeginDragHandler, IDragHandler  {

    private float offsetX;
    private float offsetY;

    public void OnBeginDrag(PointerEventData eventData)
    {
        offsetX = transform.parent.position.x - Input.mousePosition.x;
        offsetY = transform.parent.position.y - Input.mousePosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.parent.position = new Vector3(offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y);
    }
}﻿

