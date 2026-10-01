using Actors.Player.Core;
using Actors.Player.Scriptable;
using Core.Events;
using Interfaces;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Modules
{
    public class SlidingState : IState
    {
        private readonly ActionController controller;
        private readonly PlayerStats stats;
        private readonly Input input;
        private readonly CharacterController character;
        private readonly Transform transform;
        private readonly Vector3 entryVelocity;

        public SlidingState(ActionController controller, Vector3 entryVelocity)
        {
            this.controller = controller;
            stats = controller.stats;
            input = Input.Instance;
            character = controller.characterController;
            transform = controller.transform;
            this.entryVelocity = entryVelocity;
        }

        public void Enter()
        { 
        }

        public void Update()
        {
        }

        public void Exit()
        {
            
        }
    }
}
