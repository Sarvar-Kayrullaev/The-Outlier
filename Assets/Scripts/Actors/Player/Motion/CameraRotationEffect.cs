// FILE: Assets/Scripts/Actors/Player/Motion/CameraRotationEffect.cs
using Actors.Player.Actions;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Motion
{
    /// <summary>
    /// Sichqoncha inputiga asoslanib kamera va tana (body) aylanishini smooth tarzda
    /// boshqaradi, so'ngra CameraSwayEffect natijasini ustiga qo'shadi.
    /// </summary>
    public class CameraRotationEffect : ICameraMotionEffect
    {
        private const float smoothTime = 0.05f;

        private readonly Transform bodyTransform;
        private readonly Transform cameraParent;
        private readonly Input input;
        private readonly CameraSwayEffect swayEffect;

        private float xRotation;
        private float currentXRotation;
        private float currentYRotation;
        private float xRotationVelocity;
        private float yRotationVelocity;

        public CameraRotationEffect(Transform bodyTransform, Transform cameraParent, CameraSwayEffect swayEffect)
        {
            this.bodyTransform = bodyTransform;
            this.cameraParent = cameraParent;
            this.swayEffect = swayEffect;
            input = Input.Instance;
        }

        public void Tick(PlayerActionContext context)
        {
            var stats = context.stats;

            var targetX = input.rotateInput.y * (stats.mouseVerticalSensitivity.Value * stats.mouseSensitivity.Value);
            var targetY = input.rotateInput.x * (stats.mouseHorizontalSensitivity.Value * stats.mouseSensitivity.Value);

            xRotation -= targetX * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            currentXRotation = Mathf.SmoothDamp(currentXRotation, xRotation, ref xRotationVelocity, smoothTime);

            var targetBodyRotation = bodyTransform.eulerAngles.y + targetY * Time.deltaTime;
            currentYRotation = Mathf.SmoothDampAngle(bodyTransform.eulerAngles.y, targetBodyRotation, ref yRotationVelocity, smoothTime);

            var sway = swayEffect.SwayOffset;
            cameraParent.localRotation = Quaternion.Euler(currentXRotation + sway.x, sway.y, sway.z);
            bodyTransform.rotation = Quaternion.Euler(0f, currentYRotation, 0f);
        }
    }
}