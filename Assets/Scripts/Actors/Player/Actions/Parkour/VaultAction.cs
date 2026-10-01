// FILE: Assets/Scripts/Actors/Player/Actions/Parkour/VaultAction.cs
using System;
using System.Collections;
using UnityEngine;

namespace Actors.Player.Actions.Parkour
{
    /// <summary>
    /// Past to'siqlar ustidan sakrab o'tish harakati (Vault).
    /// Faqat harakat mantig'ini bajaradi - animatsiya haqida hech narsa bilmaydi,
    /// kerak bo'lsa AirborneState PlayerActionEvents orqali xabar beradi.
    /// </summary>
    public class VaultAction : IPlayerAction
    {
        private readonly Vector3 targetPosition;

        public bool LocksCharacterController => true;

        public VaultAction(Vector3 targetPosition)
        {
            this.targetPosition = targetPosition;
        }

        public void Enter(PlayerActionContext context, Action onComplete)
        {
            context.runner.StartActionCoroutine(PerformRoutine(context, onComplete));
        }

        public void Tick(PlayerActionContext context)
        {
            // Harakat to'liq Lerp-Coroutine orqali boshqariladi.
        }

        public void Cancel(PlayerActionContext context)
        {
        }

        private IEnumerator PerformRoutine(PlayerActionContext context, Action onComplete)
        {
            var transform = context.transform;
            var destination = targetPosition + Vector3.up;
            var startPosition = transform.position;
            var duration = Mathf.Max(0.05f, context.stats.vaultDuration.Value);
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / duration;
                transform.position = Vector3.Lerp(startPosition, destination, t);
                yield return null;
            }

            transform.position = destination;
            context.velocity = Vector3.zero;

            onComplete?.Invoke();
        }
    }
}