
using System;
using Interfaces;

namespace Core.Events
{
    public static class CombatEvents
    {
        public static event Action<IDamageable> OnEnemyKilled;
        public static event Action<float> OnPlayerHealthChanged; // percent 0..1
        public static event Action OnReloadStarted;
        public static event Action OnStealthKillPerformed;

        public static void RaiseEnemyKilled(IDamageable victim) => OnEnemyKilled?.Invoke(victim);
        public static void RaiseHealthChanged(float percent) => OnPlayerHealthChanged?.Invoke(percent);
        public static void RaiseReloadStarted() => OnReloadStarted?.Invoke();
        public static void RaiseStealthKill() => OnStealthKillPerformed?.Invoke();
    }
}