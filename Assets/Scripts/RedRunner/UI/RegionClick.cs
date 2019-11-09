using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class RegionClick : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onClick = new UnityEvent();

    void Start()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        RectTransform rect = GetComponent<RectTransform>();
        bc.size = new Vector2(rect.rect.width, rect.rect.height);
    }

    void OnMouseDown()
    {
        onClick.Invoke();
    }

}