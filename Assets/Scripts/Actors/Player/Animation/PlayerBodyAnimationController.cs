using Core.Events;
using UnityEngine;

namespace Actors.Player.Animation
{
    /// <summary>
    /// O'yinchining baza harakat animatsiyalarini (Idle/Walk/Run/Jump) va tegishli
    /// Animator qatlamini boshqaradi. Harakat modullariga (Actions/State'lar) to'g'ridan-to'g'ri
    /// bog'lanmaydi - faqat Core.Events.PlayerActionEvents signallarini tinglaydi.
    /// </summary>
    public class PlayerBodyAnimationController : MonoBehaviour, IAnimationLayerController
    {
        private static readonly int MoveSpeedHash = UnityEngine.Animator.StringToHash("MoveSpeed");
        private static readonly int IsGroundedHash = UnityEngine.Animator.StringToHash("IsGrounded");
        private static readonly int JumpTriggerHash = UnityEngine.Animator.StringToHash("Jump");

        [SerializeField] private UnityEngine.Animator bodyAnimator;

        private void OnEnable()
        {
            PlayerActionEvents.OnJumpStarted += HandleJumpStarted;
        }

        private void OnDisable()
        {
            PlayerActionEvents.OnJumpStarted -= HandleJumpStarted;
        }

        public void SyncWithMovement(float moveIntensity, bool isGrounded)
        {
            bodyAnimator.SetFloat(MoveSpeedHash, moveIntensity);
            bodyAnimator.SetBool(IsGroundedHash, isGrounded);
        }

        private void HandleJumpStarted()
        {
            bodyAnimator.SetTrigger(JumpTriggerHash);
        }
    }
}