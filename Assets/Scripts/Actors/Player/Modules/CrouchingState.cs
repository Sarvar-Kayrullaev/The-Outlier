// CrouchingState.cs

using Actors.Player.Core;
using Actors.Player.Scriptable;
using Interfaces;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Modules
{
    public class CrouchingState : IState
    {
        private readonly ActionController controller;
        private readonly PlayerStats stats;
        private readonly Input input;

        public CrouchingState(ActionController controller)
        {
            this.controller = controller;
            stats = controller.stats;
            input = Input.Instance;
        }

        public void Enter()
        {
            Debug.Log("Crouching");
        }

        public void Update()
        {
            controller.motor.MovementLocomotion();
            controller.cameraController.TickAll(controller.actionContext);

            // Crouch holatida ham tik qiyalikka (>= slideAutoTriggerAngle) tushilsa sirpanishga o'tiladi.
            if (controller.TryStartAutoSlide()) return;

            if (!controller.characterController.isGrounded) return;

            // Crouch tugmasi qayta bosilsa - tik turishga qaytish.
            if (input.crouchInput)
            {
                controller.ChangeState(new StandingState(controller));
            }
        }

        public void Exit()
        {
        }
    }
}