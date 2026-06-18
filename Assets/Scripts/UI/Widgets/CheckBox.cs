using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Widgets
{
    // Changed IPointerEnter/Exit to IPointerDown/Up for proper mobile touch response
    public class CheckBox : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public bool interactable = true;
        public bool isOn;

        public Image backgroundImage;
        public Image foregroundImage;

        public Color bgTrueColor = Color.green;
        public Color bgFalseColor = Color.gray;
        public Color fgTrueColor = Color.white;
        public Color fgFalseColor = new Color(1f, 1f, 1f, 0f);

        
        public Color bgPressColor = new Color(0.2f, 0.8f, 0.2f);
        public Color fgPressColor = Color.white;

        public UnityEvent<bool> onValueChanged;

        private bool isPressing = false;

#if UNITY_EDITOR
        // Updates the visual state instantly inside the Unity Editor when values change
        private void OnValidate()
        {
            UpdateVisual();
        }
#endif

        private void Start()
        {
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

        // Handles all UI color changes based on Press and Toggle states
        private void UpdateVisual()
        {
            if (backgroundImage != null)
            {
                // Prioritizes the press color if the user is holding down the button
                backgroundImage.color = isPressing ? bgPressColor : (isOn ? bgTrueColor : bgFalseColor);
            }

            if (foregroundImage != null)
            {
                // Prioritizes the press color for the checkmark/foreground as well
                foregroundImage.color = isPressing ? fgPressColor : (isOn ? fgTrueColor : fgFalseColor);
                
                // Disables the image component if it's completely transparent to save performance
                foregroundImage.enabled = foregroundImage.color.a > 0.001f;
            }
        }
    }
}