// AirborneState.cs

using Actors.Player.Actions.Parkour;
using Actors.Player.Core;
using Actors.Player.Scriptable;
using Core.Events;
using Interfaces;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Modules
{
    public class AirborneState : IState
    {
        private readonly ActionController controller;
        private readonly PlayerStats stats;
        private readonly Input input;

        private bool isPerformingParkour;

        public AirborneState(ActionController controller)
        {
            this.controller = controller;
            stats = controller.stats;
            input = Input.Instance;
        }

        public void Enter()
        {
            isPerformingParkour = false;
            Debug.Log("Airborne");
        }

        public void Update()
        {
            if (isPerformingParkour) return;

            controller.motor.MovementLocomotion();
            controller.cameraController.TickAll(controller.actionContext);

            if (input.jumpInput)
            {
                TryPerformParkour();
                return;
            }

            // isGrounded o'rniga mustaqil SphereCast tekshiruvi: tik (slopeLimit'dan katta) yuzaga tushganda
            // isGrounded doim false qaytishi mumkin va player abadiy Airborne'da qolib ketardi.
            // Tik yuzaga tushilsa darhol sirpanishga, aks holda StandingState'ga o'tiladi.
            if (controller.TryStartAutoSlide()) return;

            if (GroundSlopeUtility.TryGetGroundSlope(
                    controller.characterController, controller.transform, stats,
                    out _, out _))
            {
                controller.ChangeState(new StandingState(controller));
            }
        }

        public void Exit()
        {
        }

        private void TryPerformParkour()
        {
            if (!controller.parkourDetector.CheckObstacle(out var parkourType, out var handlingBodyPosition, out var targetPosition))
            {
                return;
            }

            switch (parkourType)
            {
                case ParkourType.Vault:
                    isPerformingParkour = true;
                    PlayerActionEvents.RaiseVaultTriggered(targetPosition);
                    controller.actionRunner.Run(new VaultAction(targetPosition), controller.actionContext, OnVaultComplete);
                    break;

                case ParkourType.Climb:
                    PlayerActionEvents.RaiseClimbTriggered(targetPosition);
                    controller.ChangeState(new ClimbingState(controller, handlingBodyPosition, targetPosition));
                    break;
            }
        }

        private void OnVaultComplete()
        {
            PlayerActionEvents.RaiseVaultCompleted();
            controller.ChangeState(new StandingState(controller));
        }
    }
}