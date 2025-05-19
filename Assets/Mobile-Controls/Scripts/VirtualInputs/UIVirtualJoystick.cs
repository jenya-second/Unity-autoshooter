using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIVirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [System.Serializable]
    public class Event : UnityEvent<Vector2> { }

    [Header("Rect References")]
    public RectTransform containerRect;
    public RectTransform handleRect;

    [Header("Settings")]
    public float joystickRange = 50f;
    public float magnitudeMultiplier = 1f;

    void Start()
    {
        joystickRange = containerRect.rect.size.x / 2;
        if (Progress.instance.Desktop)
        {
            gameObject.SetActive(false);
        }
        SetupHandle();
    }

    private void SetupHandle()
    {
        if (handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out Vector2 position);
        position = ApplySizeDelta(position);
        Vector2 clampedPosition = ClampValuesToMagnitude(position);
        if (handleRect)
        {
            UpdateHandleRectPosition(clampedPosition * joystickRange);
        }
        Vector3 outputDirection = new Vector3(clampedPosition.x, 0, clampedPosition.y);
        GameObject player = Progress.instance.player;
        if (player == null) return;
        player.GetComponent<Movement>().SetJoystickDirection(outputDirection);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject player = Progress.instance.player;
        if (player == null) return;
        player.GetComponent<Movement>().SetJoystickDirection(Vector3.zero);

        if (handleRect)
        {
            UpdateHandleRectPosition(Vector2.zero);
        }
    }

    private void UpdateHandleRectPosition(Vector2 newPosition)
    {
        handleRect.anchoredPosition = newPosition;
    }

    Vector2 ApplySizeDelta(Vector2 position)
    {
        float x = position.x / joystickRange;
        float y = position.y / joystickRange;
        return new Vector2(x, y);
    }

    Vector2 ClampValuesToMagnitude(Vector2 position)
    {
        return Vector2.ClampMagnitude(position, 1);
    }


}