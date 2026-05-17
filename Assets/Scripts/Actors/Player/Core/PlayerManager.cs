// PlayerManager.cs

using System;
using Actors.Player.Scriptable;
using Audio.Events;
using Core.Initialization;
using Core.Services;
using Interfaces;
using UnityEngine;

namespace Actors.Player.Core
{
    public class PlayerManager : MonoBehaviour, ISingle
    {
        public Transform cameraParent;
        public CharacterController characterController;
        public PlayerStats playerStats;
        public PlayerActions actions;
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
            actions = new PlayerActions(this);
            controller = new ActionController(this);
        }

        private void Update()
        {
            controller.Update();
        }
    }
}