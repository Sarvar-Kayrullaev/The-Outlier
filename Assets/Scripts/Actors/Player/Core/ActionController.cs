// ActionController.cs

using Actors.Player.Modules;
using Actors.Player.Scriptable;
using Interfaces;
using UnityEngine;

namespace Actors.Player.Core
{
    public class ActionController
    {
        public readonly Transform transform;
        public readonly Transform cameraParent;
        public readonly CharacterController characterController;
        public readonly PlayerStats stats;
        public readonly PlayerActions actions;
        
        private IState _currentState;

        public ActionController(PlayerManager manager)
        {
            actions = manager.actions;
            stats = manager.playerStats;
            transform = manager.transform;
            cameraParent = manager.cameraParent;
            characterController = manager.characterController;
            ChangeState(new StandingState(this));
        }

        public void ChangeState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}