// Actors/Player/Core/PlayerSkillEffectController.cs

using System.Collections.Generic;
using Data.Templates.Player;
using Interfaces;
using Interfaces.Interfaces;

namespace Actors.Player.Core
{
    public class PlayerSkillEffectController
    {
        private readonly PlayerManager player;
        private readonly List<ISkillEffect> activeEffects = new();

        // Tezkor so'rov uchun kesh — har safar hammasini aylanib chiqmaslik uchun
        public List<IDamageDealtModifier> damageDealtModifiers = new();
        public List<IDamageTakenModifier> damageTakenModifiers = new();

        public PlayerSkillEffectController(PlayerManager manager) => player = manager;

        public void RegisterSkill(SkillNode node)
        {
            if (node.effect == null) return;

            var instance = node.effect.CreateRuntimeInstance(); // pastda tushuntiraman
            activeEffects.Add(instance);
            instance.OnEquip(player);

            if (instance is IDamageDealtModifier d) damageDealtModifiers.Add(d);
            if (instance is IDamageTakenModifier t) damageTakenModifiers.Add(t);
        }

        // Damage tizimi shuni chaqiradi
        public float ProcessOutgoingDamage(float baseDamage, IDamageable target, DamageContext ctx)
        {
            foreach (var mod in damageDealtModifiers)
                baseDamage = mod.ModifyDamageDealt(baseDamage, target, ctx);
            return baseDamage;
        }

        public float ProcessIncomingDamage(float baseDamage, IActor attacker)
        {
            foreach (var mod in damageTakenModifiers)
                baseDamage = mod.ModifyDamageTaken(baseDamage, attacker);
            return baseDamage;
        }
    }
}