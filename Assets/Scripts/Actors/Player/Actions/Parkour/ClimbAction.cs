// FILE: Assets/Scripts/Actors/Player/Actions/Parkour/ClimbAction.cs
using System;
using System.Collections;
using Core.Events;
using UnityEngine;

namespace Actors.Player.Actions.Parkour
{
    /// <summary>
    /// Baland to'siqlarga tirmashib chiqish harakati (Climb). Ikki bosqichli Lerp:
    /// handlingBodyPosition -> targetPosition. Hand-animator bilan hech qanday
    /// bog'liqlik YO'Q - "handling nuqtaga yetildi" holati Core.Events.PlayerActionEvents
    /// orqali e'lon qilinadi, Animation modul buni mustaqil tinglaydi.
    /// </summary>
    public class ClimbAction : IPlayerAction
    {
        private readonly Vector3 handlingBodyPosition;
        private readonly Vector3 targetPosition;

        public bool LocksCharacterController => true;

        public ClimbAction(Vector3 handlingBodyPosition, Vector3 targetPosition)
        {
            this.handlingBodyPosition = handlingBodyPosition;
            this.targetPosition = targetPosition;
        }

        public void Enter(PlayerActionContext context, Action onComplete)
        {
            context.runner.StartActionCoroutine(PerformRoutine(context, onComplete));
        }

        public void Tick(PlayerActionContext context)
        {
        }

        public void Cancel(PlayerActionContext context)
        {
        }

        private IEnumerator PerformRoutine(PlayerActionContext context, Action onComplete)
        {
            var transform = context.transform;
            var stats = context.stats;

            var startPosition = transform.position;
            var distanceToHandling = Vector3.Distance(startPosition, handlingBodyPosition);
            var durationToHandling = stats.climbSpeed.Value > 0f ? distanceToHandling / stats.climbSpeed.Value : stats.climbDuration.Value;
            durationToHandling = Mathf.Max(0.1f, durationToHandling);

            var elapsed = 0f;
            while (elapsed < durationToHandling)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / durationToHandling;
                transform.position = Vector3.Lerp(startPosition, handlingBodyPosition, t);
                yield return null;
            }
            transform.position = handlingBodyPosition;

            // Ilgari shu yerda to'g'ridan-to'g'ri manager.handAnimator.Play(...) chaqirilardi.
            // Endi faqat signal beramiz - kim tinglashini bu klass bilishi shart emas.
            PlayerActionEvents.RaiseClimbHandlingReached();

            yield return new WaitForSeconds(0.15f);

            var distanceToTarget = Vector3.Distance(handlingBodyPosition, targetPosition);
            var durationToTarget = stats.climbSpeed.Value > 0f ? distanceToTarget / stats.climbSpeed.Value : stats.climbDuration.Value;
            durationToTarget = Mathf.Max(0.1f, durationToTarget);

            elapsed = 0f;
            while (elapsed < durationToTarget)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / durationToTarget;
                transform.position = Vector3.Lerp(handlingBodyPosition, targetPosition, t);
                yield return null;
            }

            transform.position = targetPosition;
            context.velocity = Vector3.zero;

            onComplete?.Invoke();
        }
    }
}