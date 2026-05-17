using Audio.Events;
using UnityEngine;

namespace Actors.Player.Core
{
    public class PlayerFootstepManager
    {
        private readonly AudioSource source;
        private readonly RandomAudioEvent defaultStepSound;

        public PlayerFootstepManager(PlayerManager manager)
        {
            source = manager.audioSource;
            defaultStepSound = manager.footstepAudioEvent;
        }
        public void PlayStepSound()
        {
            defaultStepSound.Play(source);
        }
    }
}