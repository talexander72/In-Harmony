using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class WheelControl : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private UnityEngine.UI.Image wheelImage;
    [SerializeField] private UnityEngine.UI.Image handleImage;
    [SerializeField] private Player player;
    
    private Vector2 centerPoint;
    private float wheelRadius;
    private Vector2 startDragPosition;


    private void Start()
    {
        centerPoint = wheelImage.rectTransform.position;
        wheelRadius = wheelImage.rectTransform.sizeDelta.x / 2;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - centerPoint;
        float distance = Mathf.Clamp(direction.magnitude, 0, wheelRadius);

        Vector2 clampedPosition = centerPoint + direction.normalized * distance;
        handleImage.rectTransform.position = clampedPosition;

        Vector2 normalizedDirection = direction.normalized;
        float acceleration = distance / wheelRadius;

        player.SetRotation(normalizedDirection);
        player.SetAcceleration(acceleration);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        handleImage.rectTransform.position = centerPoint;
        player.SetAcceleration(0);
        player.SetRotation(Vector2.zero);
    }
}
