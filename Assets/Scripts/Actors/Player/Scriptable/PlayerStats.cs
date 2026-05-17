using System;
using UnityEngine;

namespace Actors.Player.Scriptable
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "Player/Stats")]
    [Serializable]
    public class PlayerStats : ScriptableObject
    {
        public float walkSpeed = 5f;
        public float jumpHeight = 2f;
        public float gravity = -9.81f;
        public float mouseSensitivity = 10f;
        public float mouseVerticalSensitivity = 10f;
        public float mouseHorizontalSensitivity = 20f;
        public float swaySpeed = 0.8f;
        public float swayAmount = 0.3f;
        public float bobSpeedMultiplier = 1.5f;
        public float bobAmount = 0.1f;
        public float defaultCameraHeight = 0.8f;
    }
}
