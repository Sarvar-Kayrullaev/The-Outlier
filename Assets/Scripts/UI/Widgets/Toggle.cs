using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Widgets
{
    [RequireComponent(typeof(RectTransform))]
    // Changed IPointerEnter/Exit to IPointerDown/Up for proper mobile touch response
    public class Toggle : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool interactable = true;
        public bool isOn;

        public RectTransform toggleRect;
        public RectTransform handlerRect;
        public Image backgroundImage;
        public Image handlerImage;

        public Color bgTrueColor = Color.green;
        public Color bgFalseColor = Color.gray;
        public Color fgTrueColor = Color.white;
        public Color fgFalseColor = Color.white;

        // Renamed from Hover to Press to reflect mobile touch behavior
        public Color bgPressColor = new Color(0.2f, 0.8f, 0.2f);
        public Color fgPressColor = Color.white;

        public UnityEvent<bool> onValueChanged;

        // Tracks whether the user is currently pressing down on the toggle
        private bool isPressing = false;

#if UNITY_EDITOR
        // Updates the visual state instantly inside the Unity Editor when values change
        private void OnValidate()
        {
            if (toggleRect == null) toggleRect = GetComponent<RectTransform>();
            
            EnforceDimensions();
            UpdateVisual();
        }
#endif

        private void Start()
        {
            if (toggleRect == null) toggleRect = GetComponent<RectTransform>();
            
            EnforceDimensions();
            UpdateVisual();
        }

        // Triggered when a full click/tap cycle is completed
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable) return;

            SetIsOn(!isOn);
        }

        // Triggered the exact moment the screen is touched (Pointer Down)
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;

            isPressing = true;
            UpdateVisual();
        }

        // Triggered the exact moment the finger is lifted from the screen (Pointer Up)
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;

            isPressing = false;
            UpdateVisual();
        }

        // Public method to dynamically change the state from other scripts
        public void SetIsOn(bool value)
        {
            if (isOn == value) return;

            isOn = value;
            UpdateVisual();
            onValueChanged?.Invoke(isOn);
        }

        // Logic to automatically enforce aspect ratio dimensions (Height = Width / 2)
        private void EnforceDimensions()
        {
            if (toggleRect == null) return;

            float width = toggleRect.sizeDelta.x;
            float height = width * 0.5f;
            toggleRect.sizeDelta = new Vector2(width, height);

            if (handlerRect != null)
            {
                // The handler becomes a perfect square matching the height of the toggle
                handlerRect.sizeDelta = new Vector2(height, height);

                // Re-adjusting anchor/pivot settings for predictable positioning
                handlerRect.anchorMin = new Vector2(0f, 0.5f);
                handlerRect.anchorMax = new Vector2(0f, 0.5f);
                handlerRect.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        // Handles color changes and shifts the handle position based on state
        private void UpdateVisual()
        {
            EnforceDimensions();

            // Update colors based on Press and Toggle states
            if (backgroundImage != null)
            {
                backgroundImage.color = isPressing ? bgPressColor : (isOn ? bgTrueColor : bgFalseColor);
            }

            if (handlerImage != null)
            {
                handlerImage.color = isPressing ? fgPressColor : (isOn ? fgTrueColor : fgFalseColor);
            }

            // Reposition the internal handler along the X-axis
            if (toggleRect != null && handlerRect != null)
            {
                float height = toggleRect.sizeDelta.x * 0.5f;
                
                // Moves the handler to the right end if 'isOn' is true, otherwise keeps it at the start
                float localX = isOn ? (toggleRect.sizeDelta.x - (height * 0.5f)) : (height * 0.5f);
                
                handlerRect.anchoredPosition = new Vector2(localX, 0f);
            }
        }
    }
}