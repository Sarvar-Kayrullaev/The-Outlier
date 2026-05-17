using Actors.Player.Scriptable;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Core
{
    public class PlayerActions
    {
        private readonly Transform transform;
        private readonly Transform cameraParent;
        private readonly CharacterController character;
        private readonly PlayerStats stats;
        private readonly Input input;
        private readonly PlayerFootstepManager footstepManager;

        private Vector3 velocity;
        private const float smoothTime = 0.05f;
        private float xRotation;
        private float currentXRotation;
        private float currentYRotation;
        private float xRotationVelocity;
        private float yRotationVelocity;
        private float noiseTime;
        private float bobTimer;
        private float currentBobIntensity;
        private Vector3 lastSwayVector;
        private float lastWaveValue;
        private float currentIntensity;

        public PlayerActions(PlayerManager manager)
        {
            transform = manager.transform;
            cameraParent = manager.cameraParent;
            character = manager.characterController;
            stats = manager.playerStats;
            input = Input.Instance;
            footstepManager = manager.footstepManager;
        }

        public void MovementLocomotion(float speed)
        {
            if (character.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            var moveMag = input.moveInput.sqrMagnitude;

            var moveDirection = Vector3.zero;

            if (moveMag > 0.001f)
            {
                moveDirection = (transform.right * input.moveInput.x + transform.forward * input.moveInput.y);
                if (moveMag > 1f) moveDirection.Normalize();
            }

            velocity.y += stats.gravity * Time.deltaTime;
            var finalVelocity = (moveDirection * stats.walkSpeed) + velocity;

            character.Move(finalVelocity * Time.deltaTime);
        }

        private float lastCosValue;

        public void HandleCameraBob(float moveIntensity)
        {
            var targetIntensity = 0f;

            if (character.isGrounded && moveIntensity > 0.1f)
            {
                targetIntensity = stats.bobAmount * moveIntensity;
                var currentBobSpeed = stats.bobSpeedMultiplier * moveIntensity;
                bobTimer += Time.deltaTime * stats.walkSpeed * currentBobSpeed;
            }

            currentBobIntensity = Mathf.Lerp(currentBobIntensity, targetIntensity, Time.deltaTime * 10f);

            var currentCos = Mathf.Cos(bobTimer);

            if (character.isGrounded && moveIntensity > 0.1f)
            {
                if ((lastCosValue > 0 && currentCos <= 0) || (lastCosValue < 0 && currentCos >= 0))
                {
                    PlayFootstepSound();
                }
            }
    
            lastCosValue = currentCos;

            var currentWave = Mathf.Sin(bobTimer);
            var waveDisplay = 1f - Mathf.Abs(currentWave);
            var bobOffset = waveDisplay * currentBobIntensity;
    
            cameraParent.localPosition = new Vector3(0, stats.defaultCameraHeight + bobOffset, 0);
        }

        private void PlayFootstepSound()
        {
            footstepManager.PlayStepSound();
        }

        private const float offset1 = 1.5f;
        private const float offset2 = 3.5f;
        private const float offset3 = 5.5f;

        public void HandleCameraSway(float moveIntensity)
        {
            var dynamicSway = stats.swayAmount * (1f + moveIntensity);

            noiseTime += Time.deltaTime * stats.swaySpeed;

            var rawX = Mathf.PerlinNoise(noiseTime, offset1);
            var rawY = Mathf.PerlinNoise(offset2, noiseTime);
            var rawZ = Mathf.PerlinNoise(noiseTime + offset3, noiseTime);

            var m = dynamicSway * 2f;

            lastSwayVector.x = rawX * m - dynamicSway;
            lastSwayVector.y = rawY * m - dynamicSway;
            lastSwayVector.z = rawZ * m - dynamicSway;
        }

        public void HandleCameraRotation()
        {
            var targetX = input.rotateInput.y * (stats.mouseVerticalSensitivity * stats.mouseSensitivity);
            var targetY = input.rotateInput.x * (stats.mouseHorizontalSensitivity * stats.mouseSensitivity);

            xRotation -= targetX * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, smoothTime);

            var targetBodyRotation = transform.eulerAngles.y + targetY * Time.deltaTime;
            currentYRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetBodyRotation, ref yRotationVelocity,
                smoothTime);

            cameraParent.localRotation =
                Quaternion.Euler(currentXRotation + lastSwayVector.x, lastSwayVector.y, lastSwayVector.z);
            transform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
        }
    }
}