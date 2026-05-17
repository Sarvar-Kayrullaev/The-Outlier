using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Widgets
{
    [RequireComponent(typeof(Image))]
    public class ItemButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerExitHandler
    {
        public Color selectedPrimaryColor;
        public Color unselectedPrimaryColor;
        public Color hoverPrimaryColor;

        public bool isSecondaryColor;
        public Color selectedSecondaryColor;
        public Color unselectedSecondaryColor;
        public Color hoverSecondaryColor;
        public bool selected = false;
        
        public UnityEvent onPointerDown = new ();
        public UnityEvent onPointerUp = new ();
        public UnityEvent onClick = new ();

        #region Internal State

        public TMP_Text titleText
        {
            get
            {
                if (_titleText == null) _titleText = GetComponentInChildren<TMP_Text>();
                return _titleText;
            }
        }
        public Image iconImage
        {
            get
            {
                if (_iconImage == null) _iconImage = GetImageComponentInChild();
                return _iconImage;
            }
        }

        private Image backgroundImage
        {
            get
            {
                if (_backgroundImage == null) _backgroundImage = GetComponent<Image>();
                return _backgroundImage;
            }
        }
        
        private TMP_Text _titleText;
        private Image _iconImage;
        private Image _backgroundImage;
        private bool isHolding = false;
        #endregion
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (backgroundImage) backgroundImage.color = unselectedPrimaryColor;
            if (isSecondaryColor)
            {
                if (titleText) titleText.color = selected ? selectedSecondaryColor : unselectedSecondaryColor;
                if(iconImage) iconImage.color = selected ? selectedSecondaryColor : unselectedSecondaryColor;
            }
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        public void SetSelection(bool value)
        {
            selected = value;
            SetPrimaryColors();
            SetSecondaryColors();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isHolding = true;
            if(backgroundImage) backgroundImage.color = hoverPrimaryColor;
            if (titleText) titleText.color = hoverSecondaryColor;
            if (iconImage) iconImage.color = hoverSecondaryColor;
            onPointerDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHolding = false;
            onPointerUp.Invoke();
            SetPrimaryColors();
            SetSecondaryColors();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            isHolding = false;
            onClick.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isHolding)
            {
                SetPrimaryColors();
                SetSecondaryColors();
                isHolding = false;
            }
        }

        private void SetPrimaryColors()
        {
            if (backgroundImage) backgroundImage.color = GetSelectionPrimaryColor();
        }

        private void SetSecondaryColors()
        {
            if (titleText) titleText.color = GetSelectionSecondaryColor();
            if (iconImage) iconImage.color = GetSelectionSecondaryColor();
        }

        private Color GetSelectionPrimaryColor()
        {
            return selected ? selectedPrimaryColor : unselectedPrimaryColor;
        }
        private Color GetSelectionSecondaryColor()
        {
            return selected ? selectedSecondaryColor : unselectedSecondaryColor;
        }

        private Image GetImageComponentInChild()
        {
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out Image image))
                    return image;
            }

            return null;
        }
    }
}