using System;
using Handlers;
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
        public float minFloatValue = 0f;
        public float maxFloatValue = 1f;
        public float floatValue;
        
        public int minIntValue = 0;
        public int maxIntValue = 100;
        public int intValue;

        public Color fillerColor;
        public Color fillerHoverColor;
        public bool animation;
        public float fillerTargetHeightScale = 1.3f;
        public float animatingTime = 0.25f;

        public RectTransform containerRect;
        public RectTransform fillerRect;
        public Image fillerImage;

        public UnityEvent<float> onValueChanged;

        private bool onPressing = false;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (containerRect && fillerRect)
            {
                var initialNormalized = isWholeNumber 
                    ? Mathf.InverseLerp(minIntValue, maxIntValue, intValue) 
                    : Mathf.InverseLerp(minFloatValue, maxFloatValue, floatValue);
            
                UpdateUI(initialNormalized);
            }

            if (fillerImage)
            {
                fillerImage.color = fillerColor;
            }
        }
#endif

        public void SetValue(float value)
        {
            var initialNormalized = isWholeNumber 
                ? Mathf.InverseLerp(minIntValue, maxIntValue, value) 
                : Mathf.InverseLerp(minFloatValue, maxFloatValue, value);
            
            UpdateUI(initialNormalized);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            onPressing = true;
            UpdateValue(eventData);
            if (fillerImage) fillerImage.color = fillerHoverColor;
            if(animation) this.ScaleY(fillerRect, fillerTargetHeightScale, animatingTime);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;
            onPressing = false;
            if(animation) this.ScaleY(fillerRect, 1f, animatingTime);
            if (fillerImage) fillerImage.color = fillerColor;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!interactable || !onPressing) return;
            UpdateValue(eventData);
        }

        private void UpdateValue(PointerEventData eventData)
        {
            if (containerRect == null) return;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(containerRect, eventData.position, eventData.pressEventCamera, out var localPoint))
            {
                var width = containerRect.rect.width;
                var pivotOffset = containerRect.pivot.x * width;
                var normalizedX = Mathf.Clamp01((localPoint.x + pivotOffset) / width);

                if (isWholeNumber)
                {
                    intValue = Mathf.RoundToInt(Mathf.Lerp(minIntValue, maxIntValue, normalizedX));
                    floatValue = intValue;
                    var visualNormalized = Mathf.InverseLerp(minIntValue, maxIntValue, intValue);
                    UpdateUI(visualNormalized);
                }
                else
                {
                    floatValue = Mathf.Lerp(minFloatValue, maxFloatValue, normalizedX);
                    UpdateUI(normalizedX);
                }

                onValueChanged?.Invoke(floatValue);
            }
        }

        private void UpdateUI(float normalizedValue)
        {
            if (fillerRect != null)
            {
                fillerRect.anchorMax = new Vector2(normalizedValue, fillerRect.anchorMax.y);
            }
        }
        
        private void Start()
        {
            var initialNormalized = isWholeNumber 
                ? Mathf.InverseLerp(minIntValue, maxIntValue, intValue) 
                : Mathf.InverseLerp(minFloatValue, maxFloatValue, floatValue);
            
            UpdateUI(initialNormalized);
        }
    }
}