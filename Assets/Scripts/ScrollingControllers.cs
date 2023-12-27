using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollingControllers : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    bool isDragging = false;
    private Vector3 deltaPosition;
    private Vector3 initialPosition;
    RectTransform rectTransform;
    Canvas canvas;
    CanvasGroup canvasGroup;

    float gridSize = 600f;


    private void Awake()
    {
        // canDrag = true;
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = rectTransform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the RectTransform based on the mouse/finger position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out Vector2 localPoint);

        // Clamp the position within the square bounds
        float clampedX = Mathf.Clamp(localPoint.x, -gridSize / 2, gridSize / 2);
        float clampedY = Mathf.Clamp(localPoint.y, -gridSize / 2, gridSize / 2);

        rectTransform.localPosition = new Vector3(clampedX, clampedY, 0);
        deltaPosition = rectTransform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    { 
        isDragging=false;
        if (deltaPosition.y > initialPosition.y)
        {
            Debug.LogError("Upside");
        }
        else if (deltaPosition.y < initialPosition.y)
        {
            Debug.LogError("Downside");
        }
        if (deltaPosition.x > initialPosition.x)
        {
            Debug.LogError("RightSide");
        }
        else if (deltaPosition.x < initialPosition.x)
        {
            Debug.LogError("Leftside");
        }
    }
}
