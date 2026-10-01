using Actors.Player.Core;

namespace Interfaces
{
    namespace Interfaces
    {
        // Har bir effekt buni implement qiladi
        public interface ISkillEffect
        {
            void OnEquip(PlayerManager player);
            void OnUnequip(PlayerManager player);
        }

        // B toifa: voqea asosida
        public interface IOnKillEffect        { void OnKill(IDamageable victim); }
        public interface IOnDamageTakenEffect { void OnDamageTaken(float amount, IActor attacker); }
        public interface IOnLowHealthEffect   { void OnLowHealth(float currentPercent); }
        public interface IOnReloadEffect      { void OnReloadStart(); }

        // C toifa: qiymatni filtrlaydi/o'zgartiradi (Chain of Responsibility)
        public interface IDamageDealtModifier
        {
            float ModifyDamageDealt(float baseDamage, IDamageable target, DamageContext ctx);
        }
        public interface IDamageTakenModifier
        {
            float ModifyDamageTaken(float baseDamage, IActor attacker);
        }

        public struct DamageContext
        {
            public bool isHeadshot;
            public bool isStealthAttack;
            public bool isCrouched;
            public float targetHealthPercent;
        }
    }
}