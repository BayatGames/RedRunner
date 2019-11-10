using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class RegionClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private UnityEvent onClick = new UnityEvent();

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick.Invoke();
    }
}
