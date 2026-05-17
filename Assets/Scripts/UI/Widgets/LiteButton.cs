using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Widgets
{
    [RequireComponent(typeof(Image))]
    public class LiteButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerExitHandler
    {
        public Color primaryColor;
        public Color hoverPrimaryColor;

        public bool isSecondaryColor;
        public Color secondaryColor;
        public Color hoverSecondaryColor;
        public UnityEvent onPointerDown = new ();
        public UnityEvent onPointerUp = new ();
        public UnityEvent onClick = new ();
        #region Internal State

        private Image backgroundImage
        {
            get
            {
                if (_backgroundImage == null) _backgroundImage = GetComponent<Image>();
                return _backgroundImage;
            }
        }
        private Image iconImage
        {
            get
            {
                if (_iconImage == null) _iconImage = GetImageComponentInChild();
                return _iconImage;
            }
        }
        private TMP_Text titleText
        {
            get
            {
                if (_titleText == null) _titleText = GetComponentInChildren<TMP_Text>();
                return _titleText;
            }
        }
        
        private Image _backgroundImage;
        private Image _iconImage;
        private TMP_Text _titleText;
        private bool isHolding = false;

        #endregion
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            Awake();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        private void Awake()
        {
            if (backgroundImage) backgroundImage.color = primaryColor;
            if (isSecondaryColor)
            {
                if (titleText) titleText.color = secondaryColor;
                if (iconImage) iconImage.color = secondaryColor;
            }
            
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isHolding = true;
            if(backgroundImage) backgroundImage.color = hoverPrimaryColor;
            if (isSecondaryColor)
            {
                if (titleText) titleText.color = hoverSecondaryColor;
                if (iconImage) iconImage.color = hoverSecondaryColor;
            }
            
            onPointerDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHolding = false;
            onPointerUp.Invoke();
            if(backgroundImage) backgroundImage.color = primaryColor;
            if (isSecondaryColor)
            {
                if (titleText) titleText.color = secondaryColor;
                if (iconImage) iconImage.color = secondaryColor;
            }
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
                if(backgroundImage) backgroundImage.color = primaryColor;
                if (isSecondaryColor)
                {
                    if (titleText) titleText.color = secondaryColor;
                    if (iconImage) iconImage.color = secondaryColor;
                }
                isHolding = false;
            }
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
