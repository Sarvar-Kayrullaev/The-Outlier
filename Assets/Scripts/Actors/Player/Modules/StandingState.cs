// StandingState.cs

using Actors.Player.Core;
using Actors.Player.Scriptable;
using Interfaces;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Modules
{
    public class StandingState : IState
    {
        private readonly ActionController controller;
        private readonly PlayerStats stats;
        private readonly Transform transform;
        private readonly Transform cameraParent;
        private readonly CharacterController character;
        private readonly Input input;
        private readonly PlayerActions actions;

        private float xRotation;

        public StandingState(ActionController controller)
        {
            this.controller = controller;
            actions = controller.actions;
            stats = controller.stats;
            transform = controller.transform;
            cameraParent = controller.cameraParent;
            character = controller.characterController;
            input = Input.Instance;
        }

        public void Enter()
        {
        }

        public void Update()
        {
            actions.MovementLocomotion(1f);
            
            var moveIntensity = new Vector2(input.moveInput.x, input.moveInput.y).magnitude;
    
            actions.HandleCameraBob(moveIntensity);
            actions.HandleCameraSway(moveIntensity * 5);
            actions.HandleCameraRotation();
        }

        public void Exit()
        {
        }
    }
}