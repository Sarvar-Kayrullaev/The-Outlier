// FILE: Assets/Scripts/Actors/Player/Motion/CameraBobEffect.cs
using System;
using Actors.Player.Actions;
using UnityEngine;

namespace Actors.Player.Motion
{
    /// <summary>
    /// Yurish paytida kamerani tepa-past tebranishi (head-bob) hamda shu bilan
    /// sinxron qadam tovushi signalini chiqaradi.
    /// </summary>
    public class CameraBobEffect : ICameraMotionEffect
    {
        private const float intensityLerpSpeed = 10f;

        private readonly Transform cameraParent;
        private readonly Action onStepCycle;

        private float bobTimer;
        private float currentBobIntensity;
        private float lastCosValue;

        public CameraBobEffect(Transform cameraParent, Action onStepCycle)
        {
            this.cameraParent = cameraParent;
            this.onStepCycle = onStepCycle;
        }

        public void Tick(PlayerActionContext context)
        {
            var stats = context.stats;
            var isGrounded = context.character.isGrounded;
            var moveIntensity = context.moveIntensity;

            var targetIntensity = 0f;
            if (isGrounded && moveIntensity > 0.1f)
            {
                targetIntensity = stats.bobAmount.Value * moveIntensity;
                var currentBobSpeed = stats.bobSpeedMultiplier.Value * moveIntensity;
                bobTimer += Time.deltaTime * stats.walkSpeed.Value * currentBobSpeed;
            }

            currentBobIntensity = Mathf.Lerp(currentBobIntensity, targetIntensity, Time.deltaTime * intensityLerpSpeed);

            var currentCos = Mathf.Cos(bobTimer);
            if (isGrounded && moveIntensity > 0.1f)
            {
                if ((lastCosValue > 0 && currentCos <= 0) || (lastCosValue < 0 && currentCos >= 0))
                {
                    onStepCycle?.Invoke();
                }
            }
            lastCosValue = currentCos;

            var currentWave = Mathf.Sin(bobTimer);
            var waveDisplay = 1f - Mathf.Abs(currentWave);
            var bobOffset = waveDisplay * currentBobIntensity;

            cameraParent.localPosition = new Vector3(0, stats.defaultCameraHeight.Value + bobOffset, 0);
        }
    }
}