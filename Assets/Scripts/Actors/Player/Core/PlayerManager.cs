// PlayerManager.cs

using Actors.Player.Actions;
using Actors.Player.Animation;
using Actors.Player.Scriptable;
using Audio.Events;
using Core.Initialization;
using Interfaces;
using UnityEngine;

namespace Actors.Player.Core
{
    public class PlayerManager : MonoBehaviour, ISingle
    {
        public float backOffset;
        public float downOffset;
        public Transform cameraParent;
        public CharacterController characterController;
        public PlayerStats playerStats;
        public PlayerActionRunner actionRunner;
        public PlayerFootstepManager footstepManager;
        private ActionController controller;

        [Header("Audio")]
        public AudioSource audioSource;
        public RandomAudioEvent footstepAudioEvent;

        public void Awake()
        {
            Hub.playerManager = this;
        }

        private void Start()
        {
            footstepManager = new PlayerFootstepManager(this);
            controller = new ActionController(this);
        }

        private void Update()
        {
            controller.Update();
            //bodyAnimationController.SyncWithMovement(controller.actionContext.moveIntensity, characterController.isGrounded);
        }
    }
}