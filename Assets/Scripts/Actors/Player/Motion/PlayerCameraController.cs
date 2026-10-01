using Actors.Player.Actions;
using Actors.Player.Core;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Motion
{
    /// <summary>
    /// Kamera bilan bog'liq barcha effektlarni (Bob, Sway, Rotation) har frame to'g'ri
    /// tartibda ishga tushiruvchi composer. Locomotion State'lar (Standing/Airborne/Climbing)
    /// faqat shu klassni chaqiradi, effektlarning ichki mantig'i bilan ishlamaydi.
    /// </summary>
    public class PlayerCameraController
    {
        private readonly Input input;
        private readonly CameraSwayEffect swayEffect;
        private readonly CameraRotationEffect rotationEffect;
        private readonly CameraBobEffect bobEffect;

        public PlayerCameraController(PlayerManager manager, PlayerFootstepManager footstepManager)
        {
            input = Input.Instance;
            swayEffect = new CameraSwayEffect();
            rotationEffect = new CameraRotationEffect(manager.transform, manager.cameraParent, swayEffect);
            bobEffect = new CameraBobEffect(manager.cameraParent, footstepManager.PlayStepSound);
        }

        /// <summary>Yurgan/sakragan holatda - bob + sway + rotation.</summary>
        public void TickAll(PlayerActionContext context)
        {
            context.moveIntensity = new Vector2(input.moveInput.x, input.moveInput.y).magnitude;

            bobEffect.Tick(context);
            swayEffect.Tick(context);
            rotationEffect.Tick(context);
        }

        /// <summary>Climb kabi harakat coroutine boshqarayotgan holatlarda - faqat qarash ruxsat etiladi.</summary>
        public void TickRotationOnly(PlayerActionContext context)
        {
            rotationEffect.Tick(context);
        }
    }
}