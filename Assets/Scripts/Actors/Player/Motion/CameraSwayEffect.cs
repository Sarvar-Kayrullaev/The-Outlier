// FILE: Assets/Scripts/Actors/Player/Motion/CameraSwayEffect.cs
using Actors.Player.Actions;
using UnityEngine;

namespace Actors.Player.Motion
{
    /// <summary>
    /// Perlin-noise asosida tabiiy, tasodifiy kamera tebranishi (sway) hosil qiladi.
    /// Natija (SwayOffset) CameraRotationEffect tomonidan iste'mol qilinadi.
    /// </summary>
    public class CameraSwayEffect : ICameraMotionEffect
    {
        private const float offset1 = 1.5f;
        private const float offset2 = 3.5f;
        private const float offset3 = 5.5f;

        private float noiseTime;

        public Vector3 SwayOffset { get; private set; }

        public void Tick(PlayerActionContext context)
        {
            var stats = context.stats;
            // Original StandingState/AirborneState har doim moveIntensity*5 uzatgan - xatti-harakat saqlangan.
            var dynamicSway = stats.swayAmount.Value * (1f + context.moveIntensity * 5f);

            noiseTime += Time.deltaTime * stats.swaySpeed.Value;

            var rawX = Mathf.PerlinNoise(noiseTime, offset1);
            var rawY = Mathf.PerlinNoise(offset2, noiseTime);
            var rawZ = Mathf.PerlinNoise(noiseTime + offset3, noiseTime);

            var m = dynamicSway * 2f;

            SwayOffset = new Vector3(
                rawX * m - dynamicSway,
                rawY * m - dynamicSway,
                rawZ * m - dynamicSway);
        }
    }
}