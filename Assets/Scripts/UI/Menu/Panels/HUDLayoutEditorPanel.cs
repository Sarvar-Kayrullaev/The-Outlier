using Core.Initialization;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class HUDLayoutEditorPanel : Panel
    {
        [Header("Editor Controls")]
        [SerializeField] private LiteButton saveButton;
        [SerializeField] private LiteButton resetButton;
        [SerializeField] private LiteButton exitButton;

        [Header("Sliders (SeekBar)")]
        [SerializeField] private Slider scaleSlider;
        [SerializeField] private Slider alphaSlider;

        private HUDDraggableWidget selectedWidget;
        
        protected override void OnPanelShow()
        {
            saveButton.onClick.RemoveAllListeners();
            resetButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
            scaleSlider.onValueChanged.RemoveAllListeners();
            alphaSlider.onValueChanged.RemoveAllListeners();
            
            saveButton.onClick.AddListener(SaveLayout);
            resetButton.onClick.AddListener(ResetLayout);
            exitButton.onClick.AddListener(ExitEditor);
            scaleSlider.onValueChanged.AddListener(OnScaleSliderChanged);
            alphaSlider.onValueChanged.AddListener(OnAlphaSliderChanged);
        }

        private void OnEnable()
        {
            HUDDraggableWidget.OnWidgetSelected += HandleWidgetSelection;
            ToggleEditMode(true);
        }

        private void OnDisable()
        {
            HUDDraggableWidget.OnWidgetSelected -= HandleWidgetSelection;
            ToggleEditMode(false);
        }

        private void ToggleEditMode(bool enable)
        {
            var widgets = FindObjectsOfType<HUDDraggableWidget>();
            foreach (var widget in widgets)
            {
                widget.SetEditMode(enable);
            }
        }

        private void HandleWidgetSelection(HUDDraggableWidget widget)
        {
            selectedWidget = widget;
            if (selectedWidget != null)
            {
                scaleSlider.value = selectedWidget.transform.localScale.x;
                alphaSlider.value = selectedWidget.GetComponent<CanvasGroup>().alpha;
            }
        }

        private void OnScaleSliderChanged(float value)
        {
            if (selectedWidget != null) selectedWidget.UpdateScale(value);
        }

        private void OnAlphaSliderChanged(float value)
        {
            if (selectedWidget != null) selectedWidget.UpdateAlpha(value);
        }

        private void SaveLayout()
        {
            Hub.hudManager.SaveCurrentLayout();
            // DataService.SaveSettingsData() loyihadagi o'ziga xos chaqiruvga asosan:
            // GlobalHub yoki DataService.Instance orqali chaqiriladi
            Debug.Log("HUD Layout muvaffaqiyatli saqlandi.");
        }

        private void ResetLayout()
        {
            if (Hub.hudManager != null)
            {
                Hub.hudManager.ResetToDefault();
                if (selectedWidget != null) HandleWidgetSelection(selectedWidget);
            }
        }

        private void ExitEditor()
        {
            Hub.panelManager.CloseCurrentPanel();
        }

        protected override void OnPanelHide()
        {
            
        }
    }
}