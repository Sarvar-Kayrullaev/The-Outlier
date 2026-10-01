// PlayerActionEvents.cs
// Loyihadagi CombatEvents.cs bilan bir xil naqshda (static event-bus) yozilgan.

using System;
using UnityEngine;

namespace Core.Events
{
    public static class PlayerActionEvents
    {
        public static event Action OnJumpStarted;

        public static event Action<Vector3> OnVaultTriggered;
        public static event Action OnVaultCompleted;

        public static event Action<Vector3> OnClimbTriggered;

        // YANGI: Climb harakati "ushlash" nuqtasiga yetganda e'lon qilinadi -
        // Animation modul shu payt hand-animator'ni "climbing_end"ga o'tkazadi.
        public static event Action OnClimbHandlingReached;

        public static event Action OnClimbCompleted;

        // YANGI: Sliding boshlanishi/tugashi - Animation/Camera modullari (masalan, ekranni pastga
        // egish yoki slide-crouch animatsiyasini boshlash) shu signalni mustaqil tinglaydi.
        // SlidingState hech qanday animator yoki kamera-effekt klassini to'g'ridan-to'g'ri bilmaydi.
        public static event Action<Vector3> OnSlideStarted;   // parametr: kirish tezligi (yo'nalish + magnitude)
        public static event Action OnSlideEnded;

        public static void RaiseJumpStarted() => OnJumpStarted?.Invoke();

        public static void RaiseVaultTriggered(Vector3 targetPosition) => OnVaultTriggered?.Invoke(targetPosition);
        public static void RaiseVaultCompleted() => OnVaultCompleted?.Invoke();

        public static void RaiseClimbTriggered(Vector3 targetPosition) => OnClimbTriggered?.Invoke(targetPosition);
        public static void RaiseClimbHandlingReached() => OnClimbHandlingReached?.Invoke();
        public static void RaiseClimbCompleted() => OnClimbCompleted?.Invoke();

        public static void RaiseSlideStarted(Vector3 entryVelocity) => OnSlideStarted?.Invoke(entryVelocity);
        public static void RaiseSlideEnded() => OnSlideEnded?.Invoke();
    }
}