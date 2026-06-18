using System;
using UnityEngine;

namespace Data.Models.Core
{
    public class SettingsData
    {
        public int buildCode = 1;
        public string languageCode = "uz";
        public int frameRefreshRate = 1;
        public bool showFPS = true;
        public float masterVolume = 1f;
        public float musicVolume = 0.8f;
        public float soundEffectVolume = 0.9f;
        
        //Render
        //public int graphicsQualityIndex = 2;
        public float renderScale = 1;
        public int antiAliasingType = 0;
        public bool highDynamicRange = false;

        //Light
        public int shadow = 1;
        public int shadowResolution = 1;
        public int shadowCullingDistance = 1;
        public bool ambientOcclusion = false;

        //Environment
        public int lods = 0;
        public int skyQuality = 0;
        public int dayCycleRefreshRate = 0;
        public int textureResolution = 0;
        
        //Controller
        public HUDLayoutSettings hudLayout = new HUDLayoutSettings();
    }
    [Serializable]
    public class WidgetLayoutData
    {
        // Ekranga nisbatan foiz koeffitsiyenti (X: 0..1, Y: 0..1)
        public Vector2 normalizedPosition; 
        public Vector2 scale;
        public float alpha;

        public WidgetLayoutData(Vector2 defaultNormPos, Vector2 defaultScale, float defaultAlpha)
        {
            normalizedPosition = defaultNormPos;
            scale = defaultScale;
            alpha = defaultAlpha;
        }
    }

    [Serializable]
    // Boshlang'ich (Default) pozitsiyalar foizda beriladi:
    public class HUDLayoutSettings
    {
        public WidgetLayoutData moveJoystick = new WidgetLayoutData(new Vector2(0.5f, 0.5f), Vector2.one, 1f);
        public WidgetLayoutData rotatePanel = new WidgetLayoutData(new Vector2(0.5f, 0.5f), Vector2.one, 1f);
        public WidgetLayoutData shootButton = new WidgetLayoutData(new Vector2(0.5f, 0.5f), Vector2.one, 1f);
        public WidgetLayoutData jumpButton = new WidgetLayoutData(new Vector2(0.5f, 0.5f), Vector2.one, 1f);
        public WidgetLayoutData crouchButton = new WidgetLayoutData(new Vector2(0.5f, 0.5f), Vector2.one, 1f);
    }
}