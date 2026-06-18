using System;
using Handlers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class SeekBar : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public bool interactable = true;

        public bool isWholeNumber;
        public float minValue = 0f;
        public float maxValue = 1f;
        public float value;

        public Color fillerColor;
        public Color fillerHoverColor;
        public bool animation;
        public float fillerTargetHeightScale = 1.3f;
        public float animatingTime = 0.25f;

        public RectTransform containerRect;
        public RectTransform fillerRect;
        public Image fillerImage;
        public TMP_Text valueText;

        [Tooltip("Ready-made Prefab used as a Tooltip (Contains Image and TMP_Text components)")]
        public GameObject infoPrefab;
        [Tooltip("Fixed Y-axis offset for how high the Tooltip stays above the SeekBar")]
        public float infoYOffset = 40f;

        public UnityEvent<float> onValueChanged;

        private bool onPressing = false;
        private GameObject spawnedInfo;
        private RectTransform infoRectTransform;
        private TMP_Text infoText;

        // Helper method to snap values to 1.0 (if whole number) or 0.1 (if float)
        private float SnapValue(float rawValue)
        {
            float step = isWholeNumber ? 1f : 0.1f;
            return Mathf.Round(rawValue / step) * step;
        }

#if UNITY_EDITOR
        // Validates and clamps values automatically inside the Unity Editor
        private void OnValidate()
        {
            minValue = SnapValue(minValue);
            maxValue = SnapValue(maxValue);
            value = SnapValue(value);
            value = Mathf.Clamp(value, minValue, maxValue);

            if (containerRect && fillerRect)
            {
                var initialNormalized = Mathf.InverseLerp(minValue, maxValue, value);
                UpdateUI(initialNormalized, value);
            }

            if (fillerImage)
            {
                fillerImage.color = fillerColor;
            }
        }
#endif

        // Public method to dynamically update the SeekBar value from other scripts
        public void SetValue(float newValue)
        {
            value = SnapValue(newValue);
            value = Mathf.Clamp(value, minValue, maxValue);

            var initialNormalized = Mathf.InverseLerp(minValue, maxValue, value);
            UpdateUI(initialNormalized, value);
        }

        // Triggered when the player clicks or touches the SeekBar
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            onPressing = true;
            
            CreateInfoPopup();
            UpdateValue(eventData);
            
            if (fillerImage) fillerImage.color = fillerHoverColor;
            if (animation) this.ScaleY(fillerRect, fillerTargetHeightScale, animatingTime);
        }

        // Triggered when the player releases the click or touch
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;
            onPressing = false;
            
            DestroyInfoPopup();

            if (animation) this.ScaleY(fillerRect, 1f, animatingTime);
            if (fillerImage) fillerImage.color = fillerColor;
        }

        // Triggered continuously while the player drags the pointer across the SeekBar
        public void OnDrag(PointerEventData eventData)
        {
            if (!interactable || !onPressing) return;
            UpdateValue(eventData);
        }

        // Calculates the pointer position and updates the current value accordingly
        private void UpdateValue(PointerEventData eventData)
        {
            if (containerRect == null) return;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out var localPoint))
            {
                var width = containerRect.rect.width;
                var pivotOffset = containerRect.pivot.x * width;
                var normalizedX = Mathf.Clamp01((localPoint.x + pivotOffset) / width);

                float rawValue = Mathf.Lerp(minValue, maxValue, normalizedX);
                
                // Value is now strictly snapped to 1.0 or 0.1 intervals
                value = SnapValue(rawValue);
                value = Mathf.Clamp(value, minValue, maxValue);

                // Normalizing based on the snapped value so the visual filler matches perfectly
                var visualNormalized = Mathf.InverseLerp(minValue, maxValue, value);
                
                UpdateUI(visualNormalized, value);
                
                // Fixed section: Clean distance from containerRect corner to cursor, accounting for pivotOffset
                float targetLocalX = (visualNormalized * width) - pivotOffset;
                UpdateInfoPosition(targetLocalX + width);

                onValueChanged?.Invoke(value);
            }
        }

        // Updates the visual fill bar and the text elements
        private void UpdateUI(float normalizedValue, float displayValue)
        {
            if (fillerRect != null)
            {
                fillerRect.anchorMax = new Vector2(normalizedValue, fillerRect.anchorMax.y);
            }

            // Checks whether it should display as an integer or floating-point number
            var formattedText = isWholeNumber ? ((int)displayValue).ToString() : displayValue.ToString("F1");

            if (valueText)
            {
                valueText.text = formattedText;
            }

            if (infoText != null)
            {
                infoText.text = formattedText;
            }
        }

        // Instantiates and sets up the dynamic Tooltip popup
        private void CreateInfoPopup()
        {
            if (infoPrefab == null || containerRect == null) return;

            spawnedInfo = Instantiate(infoPrefab, containerRect);
            infoRectTransform = spawnedInfo.GetComponent<RectTransform>();
            infoText = spawnedInfo.GetComponentInChildren<TMP_Text>();

            if (infoRectTransform != null)
            {
                // SOLUTION: Strictly snap anchor points to the bottom-left corner (0, 0).
                infoRectTransform.anchorMin = new Vector2(0f, 0f);
                infoRectTransform.anchorMax = new Vector2(0f, 0f);

                // Set pivot to (0.5f, 0f): Centered horizontally on X-axis, and at the bottom on Y-axis.
                infoRectTransform.pivot = new Vector2(0.5f, 0f);
            }
        }

        // Positions the Tooltip popup precisely above the cursor position
        private void UpdateInfoPosition(float localX)
        {
            if (infoRectTransform == null) return;

            // localX coordinate aligns perfectly above the cursor without offsets
            infoRectTransform.anchoredPosition = new Vector2(localX, infoYOffset);
        }

        // Safely destroys the Tooltip popup when interaction ends
        private void DestroyInfoPopup()
        {
            if (spawnedInfo != null)
            {
                Destroy(spawnedInfo);
                spawnedInfo = null;
                infoRectTransform = null;
                infoText = null;
            }
        }
        
        private void Start()
        {
            value = SnapValue(value);
            value = Mathf.Clamp(value, minValue, maxValue);
            var initialNormalized = Mathf.InverseLerp(minValue, maxValue, value);
            UpdateUI(initialNormalized, value);
        }
    }
}