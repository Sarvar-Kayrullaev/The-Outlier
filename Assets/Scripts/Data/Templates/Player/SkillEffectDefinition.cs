using UnityEngine;
using Interfaces.Interfaces;

namespace Data.Templates.Player
{
    /// <summary>
    /// Barcha skill effektlari uchun bazaviy klass.
    /// Har bir yangi skill (Max HP, Bloodlust va h.k.) shundan voris oladi.
    /// </summary>
    public abstract class SkillEffectDefinition : ScriptableObject
    {
        [Header("Skill Effect Settings")]
        [Tooltip("SkillNode id si bilan bir xil bo'lishi tavsiya etiladi")]
        public string id;

        /// <summary>
        /// O'yin ishga tushganda (runtime) ushbu effektning ishlovchi nusxasini yaratadi.
        /// </summary>
        public abstract ISkillEffect CreateRuntimeInstance();
    }
}