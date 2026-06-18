using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace UI.Widgets
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class HUDDraggableWidget : MonoBehaviour, IDragHandler, IPointerClickHandler
    {
        public string widgetID; // "MoveJoystick", "ShootButton" va h.k.
        
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Canvas parentCanvas;
        
        public static Action<HUDDraggableWidget> OnWidgetSelected;
        private bool isEditMode = false;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            parentCanvas = GetComponentInParent<Canvas>();
        }

        public void SetEditMode(bool enabled)
        {
            isEditMode = enabled;
            // Edit rejimida tugma bosish bloklanadi, lekin drag ishlashi uchun blocksRaycasts true qoladi
            if (canvasGroup != null)
            {
                canvasGroup.blocksRaycasts = true;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isEditMode || parentCanvas == null) return;

            // Ekrandan chiqib ketmaslik mantiqi (Clamping)
            rectTransform.anchoredPosition += eventData.delta / parentCanvas.scaleFactor;
            ClampToCanvas();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isEditMode) return;
            OnWidgetSelected?.Invoke(this);
        }

        private void ClampToCanvas()
        {
            if (parentCanvas == null) return;
            RectTransform canvasRect = parentCanvas.transform as RectTransform;
            if (canvasRect == null) return;

            Vector2 minPosition = canvasRect.rect.min - rectTransform.rect.min;
            Vector2 maxPosition = canvasRect.rect.max - rectTransform.rect.max;

            Vector2 clampedPos = rectTransform.anchoredPosition;
            clampedPos.x = Mathf.Clamp(clampedPos.x, minPosition.x, maxPosition.x);
            clampedPos.y = Mathf.Clamp(clampedPos.y, minPosition.y, maxPosition.y);
            rectTransform.anchoredPosition = clampedPos;
        }

        public void UpdateScale(float scaleValue)
        {
            rectTransform.localScale = Vector3.one * scaleValue;
        }

        public void UpdateAlpha(float alphaValue)
        {
            if (canvasGroup != null) canvasGroup.alpha = alphaValue;
        }
    }
}