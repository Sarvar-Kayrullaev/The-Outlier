using System;
using System.Collections;
using System.Collections.Generic;
using Handlers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Widgets
{
    [SelectionBase]
    [DisallowMultipleComponent]
    public class Spinner : MonoBehaviour
    {
        #region Serialized Properties
        [Header("General Configuration")]
        public SpinnerType spinnerType;
        public bool interactable = true;
        public float animationDuration = 0.3f;

        [Header("Layout & Spacing")]
        public bool splitToRows;
        [Range(1, 8)] public int rowSize = 1;
        public float rowSpacing;
        public int padding;
        public float itemHeight = 30f;
        public float itemWidth = 200f;
        public float itemSpacing = 5f;
        public Color selectedItemColor = Color.white;
        public Color unselectedItemColor = Color.gray;
        public Color selectedSecondaryColor = Color.white;
        public Color unselectedSecondaryColor = Color.gray;

        [Header("Content Data")]
        public bool showSprite;
        public List<string> items = new();
        public List<SpinnerCustomModel> customItems = new();

        [Header("References")]
        public Image spinnerImage;
        public TMP_Text title;
        public LocalizedText localizedText;
        public Image image;
        public GameObject itemPrefab;
        public GameObject dropdownBackgroundPrefab;
        #endregion

        #region Internal State
        public int currentItemIndex { get; private set; }
        public bool isActivated { get; private set; }
        
        [HideInInspector] public Canvas _canvasRoot;
        
        private RectTransform _instancedSpinnerBackgroundRect;
        private GameObject _instancedDarkScreen;
        private CanvasGroup _darkScreenCanvasGroup;
        private RectTransform _root;
        private RectTransform _selfRect;
        private float _targetGraphicAlpha;
        #endregion

        public UnityEvent<int> onChanged = new();

        private void Awake()
        {
            // Agar editor'da canvas biriktirilmagan bo'lsa, dinamik qidiramiz
            if (_canvasRoot == null)
            {
                _canvasRoot = GetComponentInParent<Canvas>();
            }

            if (_canvasRoot != null)
            {
                _root = _canvasRoot.transform as RectTransform;
            }
            else
            {
                Debug.LogError("Spinner: Canvas Root topilmadi!", this);
            }

            _selfRect = transform as RectTransform;
            if (spinnerImage) _targetGraphicAlpha = spinnerImage.color.a;
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (_canvasRoot == null)
            {
                _canvasRoot = GetComponentInParent<Canvas>();
                UnityEditor.EditorUtility.SetDirty(this);
            }

            if (spinnerImage == null)
            {
                if (TryGetComponent(out Image imageComponent))
                {
                    spinnerImage = imageComponent;
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }

            if (localizedText == null && title != null)
            {
                if (title.TryGetComponent(out LocalizedText localizedTextComponent))
                {
                    localizedText = localizedTextComponent;
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }
        }
        #endif

        #region Public API
        public void Initialize(int index)
        {
            if (IsValidIndex(index)) 
                Select(index, true);
            else 
                Debug.LogError($"Invalid item index: {index}. Total items count: {GetItemsCount()}");
        }

        public void Rebuild(int index)
        {
            if (IsValidIndex(index)) 
                Select(index, false);
            else 
                Debug.LogError($"Invalid item index: {index}. Total items count: {GetItemsCount()}");
        }

        public void SetInteractable(bool parInteractable)
        {
            interactable = parInteractable;
            if (spinnerImage)
            {
                var tColor = spinnerImage.color;
                spinnerImage.color = new Color(tColor.r, tColor.g, tColor.b, interactable ? _targetGraphicAlpha : _targetGraphicAlpha / 2);
            }
        }

        public void TriggerSpinner()
        {
            if (!interactable) return;
            
            if (isActivated) CloseSpinner();
            else OpenSpinner();
        }
        #endregion

        #region Selection Logic
        private void Select(int defaultItemIndex, bool initial = false)
        {
            currentItemIndex = defaultItemIndex;
            if (!initial) onChanged.Invoke(defaultItemIndex);

            if (showSprite)
            {
                if (customItems != null && currentItemIndex < customItems.Count)
                {
                    if (localizedText) localizedText.SetText(customItems[currentItemIndex].title);
                    else if (title) title.text = customItems[currentItemIndex].title;
                    
                    if (image) image.sprite = customItems[currentItemIndex].icon;
                }
            }
            else
            {
                if (items != null && currentItemIndex < items.Count)
                {
                    if (localizedText) localizedText.SetText(items[currentItemIndex]);
                    else if (title) title.text = items[currentItemIndex];
                }
            }
        }
        #endregion

        #region Open & Close Logic
        private void OpenSpinner()
        {
            if (_root == null) return;
            isActivated = true;
            BuildSpinner();
            this.Fade(_darkScreenCanvasGroup, 1, animationDuration);
            this.ScaleY(_instancedSpinnerBackgroundRect, 1, animationDuration);
        }

        private void CloseSpinner()
        {
            isActivated = false;
            this.Fade(_darkScreenCanvasGroup, 0, animationDuration);
            this.ScaleY(_instancedSpinnerBackgroundRect, 0, animationDuration, (() =>
            {
                if (_instancedDarkScreen) Destroy(_instancedDarkScreen);
            }));
        }
        #endregion

        #region UI Building Logic
        private void BuildSpinner()
        {
            CreateDarkScreen();
            CreateBackground();
            
            var sizeDelta = CalculateSize();
            _instancedSpinnerBackgroundRect.sizeDelta = sizeDelta;
            _instancedSpinnerBackgroundRect.anchoredPosition = CalculatePosition(sizeDelta);

            SetupGridLayout();
            PopulateItems();
        }

        private void CreateDarkScreen()
        {
            _instancedDarkScreen = new GameObject("[SpinnerDarkScreen]", typeof(RectTransform), typeof(Image), typeof(CanvasGroup));
            _instancedDarkScreen.transform.SetParent(_root.transform, false);
            
            var darkScreenLiteButton = _instancedDarkScreen.AddComponent<PointerEvent>();
            if (darkScreenLiteButton) darkScreenLiteButton.onPointerDown.AddListener(CloseSpinner);
            
            var darkScreenRect = _instancedDarkScreen.GetComponent<RectTransform>();
            darkScreenRect.anchorMin = Vector2.zero;
            darkScreenRect.anchorMax = Vector2.one;
            darkScreenRect.sizeDelta = Vector2.zero;
            darkScreenRect.anchoredPosition = Vector2.zero;
            darkScreenRect.localScale = Vector3.one;
            
            var darkScreenImage = _instancedDarkScreen.GetComponent<Image>();
            darkScreenImage.color = new Color(0, 0, 0, 0.99f);
            
            _darkScreenCanvasGroup = _instancedDarkScreen.GetComponent<CanvasGroup>();
            _darkScreenCanvasGroup.interactable = true;
            _darkScreenCanvasGroup.blocksRaycasts = true;
            _darkScreenCanvasGroup.alpha = 0;
        }

        private void CreateBackground()
        {
            _instancedSpinnerBackgroundRect = Instantiate(dropdownBackgroundPrefab.transform as RectTransform, _instancedDarkScreen.transform as RectTransform);
            _instancedSpinnerBackgroundRect.anchorMin = new Vector2(0.5f, 0.5f);
            _instancedSpinnerBackgroundRect.anchorMax = new Vector2(0.5f, 0.5f);
            _instancedSpinnerBackgroundRect.pivot = new Vector2(0.5f, 0.5f);
            _instancedSpinnerBackgroundRect.localScale = new Vector3(1, 0.7f, 1);
        }

        private void SetupGridLayout()
        {
            if (_instancedSpinnerBackgroundRect.TryGetComponent(out GridLayoutGroup gridLayoutGroup))
            {
                gridLayoutGroup.cellSize = new Vector2(itemWidth, itemHeight);
                gridLayoutGroup.spacing = new Vector2(rowSpacing, itemSpacing);
                gridLayoutGroup.padding = new RectOffset(padding, padding, padding, padding);
            }
        }

        private void PopulateItems()
        {
            var index = 0;

            if (showSprite && customItems != null)
            {
                foreach (var item in customItems)
                {
                    CreateItemObject(index, item.title, item.icon);
                    index++;
                }
            }
            else if (!showSprite && items != null)
            {
                foreach (var item in items)
                {
                    CreateItemObject(index, item, null);
                    index++;
                }
            }
        }

        private void CreateItemObject(int index, string itemTitle, Sprite itemIcon)
        {
            bool isSelected = index == currentItemIndex;
            var itemRect = itemPrefab.transform as RectTransform;
            var instancedItemRect = Instantiate(itemRect, _instancedSpinnerBackgroundRect.transform as RectTransform);
            
            var itemButton = instancedItemRect.GetComponent<ItemButton>();
            if (itemButton == null) itemButton = instancedItemRect.AddComponent<ItemButton>();
            
            var chooseIndex = index;
            itemButton.onClick.RemoveAllListeners();
            itemButton.onClick.AddListener(() => Select(chooseIndex));
            itemButton.onClick.AddListener(CloseSpinner);
            
            itemButton.selectedPrimaryColor = selectedItemColor;
            itemButton.unselectedPrimaryColor = unselectedItemColor;
            itemButton.hoverPrimaryColor = unselectedSecondaryColor;
            itemButton.isSecondaryColor = true;
            itemButton.selectedSecondaryColor = selectedSecondaryColor;
            itemButton.unselectedSecondaryColor = unselectedSecondaryColor;
            itemButton.hoverSecondaryColor = unselectedItemColor;
            itemButton.SetSelection(isSelected);
            
            var textTransform = instancedItemRect.Find("Text");
            if (textTransform && textTransform.TryGetComponent(out LocalizedText itemText))
                itemText.SetText(itemTitle);
            else if (textTransform && textTransform.TryGetComponent(out TMP_Text tmpText))
                tmpText.text = itemTitle;
            
            if (showSprite && itemIcon != null)
            {
                var imgTransform = instancedItemRect.Find("Image");
                if (imgTransform && imgTransform.TryGetComponent(out Image img))
                    img.sprite = itemIcon;
            }

            instancedItemRect.sizeDelta = new Vector2(itemWidth, itemHeight);
        }
        #endregion

        #region Mathematics & Utilities Logic
        private int GetItemsCount()
        {
            return showSprite ? (customItems?.Count ?? 0) : (items?.Count ?? 0);
        }

        private bool IsValidIndex(int index)
        {
            int count = GetItemsCount();
            return index >= 0 && index < count;
        }

        private Vector2 CalculateSize()
        {
            var itemCount = GetItemsCount();
            var sizeDelta = Vector2.zero;

            if (spinnerType == SpinnerType.Dialog)
            {
                var singleRowSize = Mathf.Ceil((float)itemCount / (float)rowSize);
                var clampedHeight = _root.rect.height * 0.8f;
                float width, height;

                if (splitToRows)
                {
                    width = (itemWidth * rowSize) + (padding * 2) + (rowSpacing * (rowSize - 1));
                    height = (itemHeight * singleRowSize) + (padding * 2) + (itemSpacing * (singleRowSize - 1));
                }
                else
                {
                    width = itemWidth + (padding * 2);
                    height = (itemHeight * itemCount) + (padding * 2) + (itemSpacing * (itemCount - 1));
                }

                sizeDelta.x = Mathf.Clamp(width, itemWidth + (padding * 2), _root.rect.width);
                sizeDelta.y = Mathf.Clamp(height, itemHeight + (padding * 2), clampedHeight);
            }
            else
            {
                sizeDelta.x = itemWidth + (padding * 2);
                sizeDelta.y = (itemHeight * itemCount) + (padding * 2) + (itemSpacing * (itemCount - 1));
            }

            return sizeDelta;
        }

        private Vector2 CalculatePosition(Vector2 sizeDelta)
        {
            Vector2 backgroundPosition;
            if (spinnerType == SpinnerType.Dropdown)
            {
                backgroundPosition = GetScreenPosition(_selfRect);
                
                var isOffScreenBottom = backgroundPosition.y - (sizeDelta.y / 2) < 0;
                var isOffScreenTop = backgroundPosition.y + (sizeDelta.y / 2) > _root.rect.height;
                
                if (isOffScreenBottom || isOffScreenTop)
                {
                    var clampedPositionY = isOffScreenBottom ? ((sizeDelta.y / 2) + 10) : _root.rect.height - ((sizeDelta.y / 2) + 10);
                    backgroundPosition = new Vector2(backgroundPosition.x, clampedPositionY);
                }
            }
            else
            {
                backgroundPosition = Vector2.zero; // Dialog markazda ochiladi (Anchor 0.5 o'rnatilgani sababli)
            }

            return backgroundPosition;
        }

        private Vector2 GetScreenPosition(RectTransform rectTransform)
        {
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            
            // UI elementining markazini hisoblash
            Vector3 centerWorld = (worldCorners[0] + worldCorners[2]) / 2f;
            Vector2 localPoint = _root.InverseTransformPoint(centerWorld);
            
            return localPoint;
        }
        #endregion
    }

    #region Models
    public enum SpinnerType { Dropdown, Dialog }

    [Serializable]
    public class SpinnerCustomModel
    {
        public string title;
        public Sprite icon;
    }
    #endregion
}