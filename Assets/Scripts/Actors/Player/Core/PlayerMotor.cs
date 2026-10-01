// FILE: Assets/Scripts/Actors/Player/Core/PlayerMotor.cs
using Actors.Player.Actions;
using Actors.Player.Scriptable;
using UnityEngine;
using Input = Actors.Player.Controller.Input;

namespace Actors.Player.Core
{
    /// <summary>
    /// Grounded harakatlanish, gravitatsiya va sakrash uchun sof fizika hisob-kitoblarini
    /// bajaradi. Ilgari PlayerActions ichida bo'lgan MovementLocomotion/Jump mantig'i
    /// shu yerga ajratilgan - Parkour va Kamera endi bu klassdan mustaqil.
    /// </summary>
    public class PlayerMotor
    {
        private readonly Transform transform;
        private readonly CharacterController character;
        private readonly PlayerStats stats;
        private readonly Input input;
        private readonly PlayerActionContext context;

        public PlayerMotor(PlayerManager manager, PlayerActionContext context)
        {
            transform = manager.transform;
            character = manager.characterController;
            stats = manager.playerStats;
            input = Input.Instance;
            this.context = context;
        }

        public void MovementLocomotion()
        {
            if (character.isGrounded && context.velocity.y < 0)
            {
                context.velocity.y = -2f;
            }

            var moveMag = input.moveInput.sqrMagnitude;
            var moveDirection = Vector3.zero;

            if (moveMag > 0.001f)
            {
                moveDirection = (transform.right * input.moveInput.x + transform.forward * input.moveInput.y);
                if (moveMag > 1f) moveDirection.Normalize();
            }

            context.velocity.y += stats.gravity.Value * Time.deltaTime;
            var finalVelocity = (moveDirection * stats.walkSpeed.Value) + context.velocity;

            character.Move(finalVelocity * Time.deltaTime);
        }

        public void Jump()
        {
            context.velocity.y = Mathf.Sqrt(2f * -stats.gravity.Value * stats.jumpHeight.Value);
        }
    }
}