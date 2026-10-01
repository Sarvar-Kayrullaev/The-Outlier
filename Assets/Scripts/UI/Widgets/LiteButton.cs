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
        [Header("Interactable Settings")]
        [SerializeField] private bool _interactable = true;
        public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.25f);

        [Header("Primary Colors (Background)")]
        public Color primaryColor = Color.white;
        public Color hoverPrimaryColor;

        [Header("Secondary Colors (Text/Icon)")]
        public bool isSecondaryColor;
        public Color secondaryColor = Color.black;
        public Color hoverSecondaryColor;

        [Header("Events")]
        public UnityEvent onPointerDown = new ();
        public UnityEvent onPointerUp = new ();
        public UnityEvent onClick = new ();

        // Interactable uchun property (kod orqali o'zgartirish uchun)
        public bool interactable
        {
            get => _interactable;
            set
            {
                if (_interactable == value) return;
                _interactable = value;
                UpdateVisualState();
            }
        }

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
            // Unity Editor'da interactable o'zgarganda vizual holatni yangilash
            UpdateVisualState();
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        private void Awake()
        {
            UpdateVisualState();
        }

        // Vizual holatni yangilovchi alohida metod
        public void UpdateVisualState()
        {
            if (!_interactable)
            {
                if (backgroundImage) backgroundImage.color = disabledColor;
                if (isSecondaryColor)
                {
                    if (titleText) titleText.color = disabledColor;
                    if (iconImage) iconImage.color = disabledColor;
                }
            }
            else
            {
                if (backgroundImage) backgroundImage.color = primaryColor;
                if (isSecondaryColor)
                {
                    if (titleText) titleText.color = secondaryColor;
                    if (iconImage) iconImage.color = secondaryColor;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!_interactable) return; // Agar faol bo'lmasa, ishlamaydi

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
            if (!_interactable) return; // Agar faol bo'lmasa, ishlamaydi

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
            if (!_interactable) return; // Agar faol bo'lmasa, ishlamaydi

            isHolding = false;
            onClick.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_interactable) return; // Agar faol bo'lmasa, ishlamaydi

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