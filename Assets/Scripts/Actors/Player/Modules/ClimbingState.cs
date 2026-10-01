using Actors.Player.Actions.Parkour;
using Actors.Player.Core;
using Actors.Player.Scriptable;
using Core.Events;
using Interfaces;
using UnityEngine;

namespace Actors.Player.Modules
{
    public class ClimbingState : IState
    {
        private readonly ActionController controller;
        private readonly PlayerStats stats;
        private readonly Vector3 handlingBodyPosition;
        private readonly Vector3 targetPosition;

        private bool isClimbComplete;

        public ClimbingState(ActionController controller, Vector3 handlingBodyPosition, Vector3 targetPosition)
        {
            this.controller = controller;
            stats = controller.stats;
            this.handlingBodyPosition = handlingBodyPosition;
            this.targetPosition = targetPosition;
        }

        public void Enter()
        {
            Debug.Log("Climbing");
            isClimbComplete = false;
            controller.actionRunner.Run(
                new ClimbAction(handlingBodyPosition, targetPosition),
                controller.actionContext,
                OnClimbComplete);
        }

        public void Update()
        {
            // Harakatning o'zi ClimbAction ichidagi coroutine orqali boshqariladi.
            // Shu payt faqat kamera aylanishiga ruxsat beramiz.
            controller.cameraController.TickRotationOnly(controller.actionContext);
        }

        public void Exit()
        {
        }

        private void OnClimbComplete()
        {
            if (isClimbComplete) return;
            isClimbComplete = true;

            PlayerActionEvents.RaiseClimbCompleted();
            controller.ChangeState(new StandingState(controller));
        }
    }
}