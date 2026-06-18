using UnityEngine;
using Data.Models.Core;
using System.Collections.Generic;
using Core.Initialization;
using Core.Services;
using Interfaces;

namespace UI.Widgets
{
    public class HUDManager : MonoBehaviour, ISingle
    {
        [Header("HUD Elements")]
        [SerializeField] private HUDDraggableWidget moveJoystick;
        [SerializeField] private HUDDraggableWidget rotatePanel;
        [SerializeField] private HUDDraggableWidget shootButton;
        [SerializeField] private HUDDraggableWidget jumpButton;
        [SerializeField] private HUDDraggableWidget crouchButton;

        private Dictionary<string, HUDDraggableWidget> widgetMap;

        private void Start()
        {
            InitializeWidgetMap();
            LoadLayout();
        }

        private void InitializeWidgetMap()
        {
            widgetMap = new Dictionary<string, HUDDraggableWidget>
            {
                { "MoveJoystick", moveJoystick },
                { "RotatePanel", rotatePanel },
                { "ShootButton", shootButton },
                { "JumpButton", jumpButton },
                { "CrouchButton", crouchButton }
            };
        }

        public void LoadLayout()
        {
            // Loyihadagi DataService'dan ma'lumotni olish (Arxitekturangizga mos holda DataService.SettingsData olinadi)
            HUDLayoutSettings settings = Hub.dataService.GetSettingsData().hudLayout;

            ApplyDataToWidget(moveJoystick, settings.moveJoystick);
            ApplyDataToWidget(rotatePanel, settings.rotatePanel);
            ApplyDataToWidget(shootButton, settings.shootButton);
            ApplyDataToWidget(jumpButton, settings.jumpButton);
            ApplyDataToWidget(crouchButton, settings.crouchButton);
        }

        private void ApplyDataToWidget(HUDDraggableWidget widget, WidgetLayoutData data)
        {
            if (widget == null || data == null) return;
            RectTransform rect = widget.transform as RectTransform;
            RectTransform parentRect = rect.parent as RectTransform; // Ota-ona (Canvas/Safe Area)

            if (rect != null && parentRect != null)
            {
                // Foizni ota-ona o'lchamiga ko'paytirib, pikseldagi pozitsiyani topamiz
                Vector2 targetPosition = new Vector2(
                    data.normalizedPosition.x * parentRect.rect.width,
                    data.normalizedPosition.y * parentRect.rect.height
                );
        
                // Pivotga qarab og'ishni (offset) hisobga olsak yanada aniq tushadi
                rect.anchoredPosition = targetPosition - (parentRect.rect.size * rect.pivot);
                rect.localScale = new Vector3(data.scale.x, data.scale.y, 1f);
            }
            widget.UpdateAlpha(data.alpha);
        }

        public void SaveCurrentLayout()
        {
            var settings = Hub.dataService.GetSettingsData().hudLayout;

            settings.moveJoystick = GetWidgetData(moveJoystick);
            settings.rotatePanel = GetWidgetData(rotatePanel);
            settings.shootButton = GetWidgetData(shootButton);
            settings.jumpButton = GetWidgetData(jumpButton);
            settings.crouchButton = GetWidgetData(crouchButton);

            Hub.dataService.GetSettingsData().hudLayout = settings;

            // Loyihangizning markaziy saqlash funksiyasi chaqiriladi
            Hub.dataService.SaveSettingsData();
        }

        private WidgetLayoutData GetWidgetData(HUDDraggableWidget widget)
        {
            RectTransform rect = widget.transform as RectTransform;
            RectTransform parentRect = rect.parent as RectTransform;
            CanvasGroup group = widget.GetComponent<CanvasGroup>();

            if (rect != null && parentRect != null)
            {
                // Piksel pozitsiyasini ota-ona o'lchamiga bo'lib, foizni (0..1) aniqlaymiz
                Vector2 absolutePos = rect.anchoredPosition + (parentRect.rect.size * rect.pivot);
                Vector2 normalizedPos = new Vector2(
                    Mathf.Clamp01(absolutePos.x / parentRect.rect.width),
                    Mathf.Clamp01(absolutePos.y / parentRect.rect.height)
                );

                return new WidgetLayoutData(normalizedPos, (Vector2)rect.localScale, group != null ? group.alpha : 1f);
            }
            return new WidgetLayoutData(Vector2.zero, Vector2.one, 1f);
        }

        public void ResetToDefault()
        {
            // Default yangi obyekt yaratilganda konstruktordagi boshlang'ich qiymatlar olinadi
            HUDLayoutSettings defaultSettings = new HUDLayoutSettings();
            
            ApplyDataToWidget(moveJoystick, defaultSettings.moveJoystick);
            ApplyDataToWidget(rotatePanel, defaultSettings.rotatePanel);
            ApplyDataToWidget(shootButton, defaultSettings.shootButton);
            ApplyDataToWidget(jumpButton, defaultSettings.jumpButton);
            ApplyDataToWidget(crouchButton, defaultSettings.crouchButton);
        }
    }
}