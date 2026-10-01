using Actors.Player.Core;
using Data.Templates.Player;
using Interfaces.Interfaces;
using UnityEngine;

namespace Data.Templates.Skills
{
    [CreateAssetMenu(fileName = "MaxHPEffect", menuName = "SkillTree/Effects/Max HP")]
    public class MaxHPEffect : SkillEffectDefinition
    {
        [Header("Modifiers")]
        public float healthPercentageBonus = 0.15f; // +15% HP

        public override ISkillEffect CreateRuntimeInstance() => new Runtime(this);

        // Ichki klass: Runtime holatida aynan nima ish qilinishini belgilaydi
        private class Runtime : ISkillEffect
        {
            private readonly MaxHPEffect _def;

            public Runtime(MaxHPEffect def) => _def = def;

            public void OnEquip(PlayerManager player)
            {
                // DIQQAT: Kodingizda PlayerStats ichiga ModifiableStat tizimini qo'shgan bo'lishingiz kerak
                // player.playerStats.maxHealth.SetModifier(_def.id, percent: _def.healthPercentageBonus);
                Debug.Log($"[Skill] {_def.id} ishga tushdi: HP {_def.healthPercentageBonus * 100}% ga oshdi.");
            }

            public void OnUnequip(PlayerManager player)
            {
                // player.playerStats.maxHealth.RemoveModifier(_def.id);
            }
        }
    }
}