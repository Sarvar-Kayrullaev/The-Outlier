using System;
using TMPro;
using UnityEngine;

namespace Other
{
    public class FPSCounter : MonoBehaviour
    {
        public TMP_Text counterText;
        public float updateInterval = 0.2f;

        private float _accumulatedTime = 0f;
        private int _frameCount = 0;
        private float _timeLeft;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
        }

        private void Start()
        {
            _timeLeft = updateInterval;
            
        }

        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            // This calculates the reciprocal of the frame time
            _accumulatedTime += 1.0f / Time.unscaledDeltaTime;
            _frameCount++;

            if (_timeLeft <= 0.0)
            {
                // Calculate the average and cast it to an int to remove decimals
                int finalFps = (int)(_accumulatedTime / _frameCount);

                counterText.text = $"FPS: {finalFps}";

                // Reset
                _timeLeft = updateInterval;
                _accumulatedTime = 0f;
                _frameCount = 0;
            }
        }
    }
}