// StandingState.cs

using Actors.Player.Core;
using Actors.Player.Scriptable;
using Core.Events;
using Interfaces;
using RuntimeDebugger;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Modules
{
    public class StandingState : IState
    {
        private readonly ActionController controller;
        private readonly PlayerStats stats;
        private readonly Input input;

        public StandingState(ActionController controller)
        {
            this.controller = controller;
            stats = controller.stats;
            input = Input.Instance;
        }

        public void Enter()
        {
            DebugSystem.PerformanceStart("StandEntering", "Stand Entering");
            Debug.Log("Standing");
            DebugSystem.Message("Standing State Enter");
            DebugSystem.PerformanceEnd("StandEntering");
        }

        public void Update()
        {
            DebugSystem.Log("Standing State Updating");
            DebugSystem.Block().AddBlock(controller.manager.name).AddText("Position: ").AddBlock(controller.manager.transform.position.ToString());
            controller.motor.MovementLocomotion();
            controller.cameraController.TickAll(controller.actionContext);

            if (controller.characterController.isGrounded && input.jumpInput)
            {
                controller.motor.Jump();
                PlayerActionEvents.RaiseJumpStarted();
                controller.ChangeState(new AirborneState(controller));
                return;
            }

            if (controller.TryStartAutoSlide()) return;

            if (!controller.characterController.isGrounded) return;

            TryEnterManualSliding();
        }

        public void Exit()
        {
        }

        private void TryEnterManualSliding()
        {
            if (!input.crouchInput) return;

            // Kirishda Y tezligi ham uzatiladi - SlidingState uni qiyalik tekisligiga proyeksiya qiladi.
            var fullVelocity = controller.characterController.velocity;
            var horizontalMagnitude = new Vector3(fullVelocity.x, 0f, fullVelocity.z).magnitude;

            if (horizontalMagnitude >= stats.slideMinEntrySpeed.Value)
            {
                controller.ChangeState(new SlidingState(controller, fullVelocity));
            }
            else
            {
                controller.ChangeState(new CrouchingState(controller));
            }
        }
    }
}