using Core.Initialization;
using Data.Models.Core;
using UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu.Panels
{
    public class SettingsPanel : Panel
    {
        [Header("Options")]
        [Space]
        [SerializeField] private Spinner language;
        [SerializeField] private Spinner frameRefreshRate;
        [SerializeField] private CheckBox showFPS;
        [Space]
        [SerializeField] private SeekBar masterVolume;
        [SerializeField] private SeekBar musicVolume;
        [SerializeField] private SeekBar soundEffectVolume;
        
        [Header("Graphics")]
        [Space]
        [SerializeField] private SeekBar renderScale;
        [SerializeField] private Spinner antiAliasingType;
        [SerializeField] private CheckBox highDynamicRange;
        [Space]
        [SerializeField] private Spinner shadow;
        [SerializeField] private Spinner shadowResolution;
        [SerializeField] private Spinner shadowCullingDistance;
        [SerializeField] private CheckBox ambienceOcclusion;
        [Space]
        [SerializeField] private Spinner lods;
        [SerializeField] private Spinner skyQuality;
        [SerializeField] private Spinner dayCycleRefreshRate;
        [SerializeField] private Spinner textureResolution;

        protected override void OnPanelShow()
        {
            LoadCurrentSettings();
        }

        protected override void OnPanelHide()
        {
            // Save settings and apply modifications
            Hub.dataService.SaveSettingsData();
        }

        private void LoadCurrentSettings()
        {
            // Load current settings in here
            var settingsData = Hub.dataService.GetSettingsData();
            InitSettingsUI(settingsData);
        }

        private void InitSettingsUI(SettingsData settingsData)
        {
            // Language
            language.Initialize(Hub.localizationService.GetIndexFromLanguageCode(settingsData.languageCode));
            language.onChanged.RemoveAllListeners();
            language.onChanged.AddListener((languageIndex =>
            {
                settingsData.languageCode = Hub.localizationService.GetLanguageCodeFromIndex(languageIndex);
                Hub.localizationService.SetLanguage(settingsData.languageCode);
            }));
            
            // Frame Refresh Rate
            frameRefreshRate.Initialize(settingsData.frameRefreshRate);
            frameRefreshRate.onChanged.RemoveAllListeners();
            frameRefreshRate.onChanged.AddListener((value =>
            {
                settingsData.frameRefreshRate = value;
            }));
            
            // Show FPS
            showFPS.SetIsOn(settingsData.showFPS);
            showFPS.onValueChanged.RemoveAllListeners();
            showFPS.onValueChanged.AddListener(value =>
            {
                settingsData.showFPS = value;
            });
            
            // Master Volume
            masterVolume.SetValue(settingsData.masterVolume);
            masterVolume.onValueChanged.RemoveAllListeners();
            masterVolume.onValueChanged.AddListener(value =>
            {
                settingsData.masterVolume = value;
            });
            
            // Music Volume
            musicVolume.SetValue(settingsData.musicVolume);
            musicVolume.onValueChanged.RemoveAllListeners();
            musicVolume.onValueChanged.AddListener(value =>
            {
                settingsData.musicVolume = value;
            });
            
            // Sound Effect Volume
            soundEffectVolume.SetValue(settingsData.soundEffectVolume);
            soundEffectVolume.onValueChanged.RemoveAllListeners();
            soundEffectVolume.onValueChanged.AddListener((value =>
            {
                settingsData.soundEffectVolume = value;
            }));
            
            // Render Scale
            renderScale.SetValue(settingsData.renderScale);
            renderScale.onValueChanged.RemoveAllListeners();
            renderScale.onValueChanged.AddListener(value =>
            {
                settingsData.renderScale = value;
            });
            
            // Anti Aliasing Type
            antiAliasingType.Initialize(settingsData.antiAliasingType);
            antiAliasingType.onChanged.RemoveAllListeners();
            antiAliasingType.onChanged.AddListener(value =>
            {
                settingsData.antiAliasingType = value;
            });
            
            // High Dynamic Range
            highDynamicRange.SetIsOn(settingsData.highDynamicRange);
            highDynamicRange.onValueChanged.RemoveAllListeners();
            highDynamicRange.onValueChanged.AddListener((value =>
            {
                settingsData.highDynamicRange = value;
            }));
            
            // Shadow
            shadow.Initialize(settingsData.shadow);
            shadow.onChanged.RemoveAllListeners();
            shadow.onChanged.AddListener(value =>
            {
                settingsData.shadow = value;
            });
            
            // Shadow Resolution
            shadowResolution.Initialize(settingsData.shadowResolution);
            shadowResolution.onChanged.RemoveAllListeners();
            shadowResolution.onChanged.AddListener(value =>
            {
                settingsData.shadowResolution = value;
            });
            
            // Shadow Culling Distance
            shadowCullingDistance.Initialize(settingsData.shadowCullingDistance);
            shadowCullingDistance.onChanged.RemoveAllListeners();
            shadowCullingDistance.onChanged.AddListener(value =>
            {
                settingsData.shadowCullingDistance = value;
            });
            
            // Ambience Occlusion
            ambienceOcclusion.SetIsOn(settingsData.ambientOcclusion);
            ambienceOcclusion.onValueChanged.RemoveAllListeners();
            ambienceOcclusion.onValueChanged.AddListener(value =>
            {
                settingsData.ambientOcclusion = value;
            });
            
            // Level Of Details
            lods.Initialize(settingsData.lods);
            lods.onChanged.RemoveAllListeners();
            lods.onChanged.AddListener((value =>
            {
                settingsData.lods = value;
            }));
            
            // Sky Quality 
            skyQuality.Initialize(settingsData.skyQuality);
            skyQuality.onChanged.RemoveAllListeners();
            skyQuality.onChanged.AddListener(value =>
            {
                settingsData.skyQuality = value;
            });
            
            // Day Cycle Refresh Rate
            dayCycleRefreshRate.Initialize(settingsData.dayCycleRefreshRate);
            dayCycleRefreshRate.onChanged.RemoveAllListeners();
            dayCycleRefreshRate.onChanged.AddListener(value =>
            {
                settingsData.dayCycleRefreshRate = value;
            });
            
            // Texture Resolution
            textureResolution.Initialize(settingsData.textureResolution);
            textureResolution.onChanged.RemoveAllListeners();
            textureResolution.onChanged.AddListener((value =>
            {
                settingsData.textureResolution = value;
            }));
        }
    }
}