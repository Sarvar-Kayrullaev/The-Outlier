using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Panels
{
    public class SettingsPanel : Panel
    {
        [Header("Audio Settings")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        
        [Header("Graphics Settings")]
        [SerializeField] private Spinner qualityDropdown;

        protected override void OnPanelShow()
        {
            LoadCurrentSettings();
        }

        protected override void OnPanelHide()
        {
            // Save settings and apply modifications
        }

        private void LoadCurrentSettings()
        {
            // Load current settings in here
        }
    }
}