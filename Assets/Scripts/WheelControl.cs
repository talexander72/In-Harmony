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

    private void Start()
    {
        centerPoint = wheelImage.rectTransform.position;
        wheelRadius = wheelImage.rectTransform.sizeDelta.x / 2;
    }


    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - centerPoint;
        if (direction.magnitude > wheelRadius) {
            direction = direction.normalized * wheelRadius;
        }

        handleImage.rectTransform.position = centerPoint + direction;
        float distance = direction.magnitude / wheelRadius;
        player.SetAcceleration(distance);
        player.SetRotation(direction.normalized);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handleImage.rectTransform.position = centerPoint;
        player.SetAcceleration(0);
        player.SetRotation(Vector2.zero);
    }
}
