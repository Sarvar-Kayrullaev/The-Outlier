using Core.Events;
using UnityEngine;

namespace Actors.Player.Animation
{
    /// <summary>
    /// Qo'l (Hand) Animator'ini - Vault/Climb kabi ustki qatlam animatsiyalarini boshqaradi.
    /// Ilgari PlayerActions ichida to'g'ridan-to'g'ri (manager.handAnimator.Play(...)) chaqirilib,
    /// Action va Animatsiya orasida qattiq bog'liqlik yaratgan edi. Endi bu klass faqat
    /// Core.Events.PlayerActionEvents orqali xabardor bo'ladi - VaultAction/ClimbAction bu
    /// klass haqida umuman bilmaydi (Event-Driven Decoupling).
    /// </summary>
    public class PlayerHandAnimationController : MonoBehaviour, IAnimationLayerController
    {
        [SerializeField] private UnityEngine.Animator handAnimator;

        private void OnEnable()
        {
            PlayerActionEvents.OnClimbTriggered += HandleClimbTriggered;
            PlayerActionEvents.OnClimbHandlingReached += HandleClimbHandlingReached;
        }

        private void OnDisable()
        {
            PlayerActionEvents.OnClimbTriggered -= HandleClimbTriggered;
            PlayerActionEvents.OnClimbHandlingReached -= HandleClimbHandlingReached;
        }

        public void SyncWithMovement(float moveIntensity, bool isGrounded)
        {
            // Hand qatlami grounded holatiga bevosita bog'liq emas - hozircha bo'sh qoldirilgan.
        }

        private void HandleClimbTriggered(Vector3 targetPosition)
        {
            handAnimator.Play("climbing_start");
        }

        private void HandleClimbHandlingReached()
        {
            handAnimator.Play("climbing_end");
        }
    }
}