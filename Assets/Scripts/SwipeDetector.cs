using System.Collections;
using System.Collections.Generic;
using  UnityEngine;
using UnityEngine.EventSystems;


public class SwipeDetector : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Player player;
    private Vector2 startDragPosition;
    private bool isDragging = false;
    private const float swipeThreshold = 50f; // Adjust the threshold as needed

    public void OnBeginDrag(PointerEventData eventData)
    {
        startDragPosition = eventData.position;
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            Vector2 dragDelta = eventData.position - startDragPosition;
            if (dragDelta.y > swipeThreshold)
            {
                player.TriggerPulse();
                isDragging = false; // Prevent multiple triggers in one drag
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }
}