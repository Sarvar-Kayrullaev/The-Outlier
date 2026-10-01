// FILE: Assets/Scripts/Other/ModifiableStat.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Other
{
    /// <summary>
    /// Bitta modifikatorni ifodalaydi: kim qo'shgani (SourceId), qancha "flat" (o'zgarmas)
    /// va qancha "percent" (foizli) bonus berayotgani. Skill/Buff/Equipment - barchasi
    /// shu struktura orqali statlarga ta'sir qiladi.
    /// </summary>
    [Serializable]
    public struct StatModifier
    {
        public string SourceId;
        public float Flat;
        public float Percent;

        public StatModifier(string sourceId, float flat = 0f, float percent = 0f)
        {
            SourceId = sourceId;
            Flat = flat;
            Percent = percent;
        }
    }

    /// <summary>
    /// Inspector'da oddiy `float` kabi ko'rinadigan, lekin runtime'da skill/buff/equipment
    /// tomonidan flat va percent modifikatorlar bilan boyitilishi mumkin bo'lgan stat turi.
    /// Formula: Value = (BaseValue + SUM(flat)) * (1 + SUM(percent)).
    /// Natija har safar qayta hisoblanmasligi uchun dirty-flag orqali keshlanadi.
    /// </summary>
    [Serializable]
    public class ModifiableStat
    {
        [UnityEngine.SerializeField] private float baseValue;

        private readonly Dictionary<string, StatModifier> modifiers = new();

        private float cachedValue;
        private bool isDirty = true;

        public ModifiableStat() { }

        public ModifiableStat(float baseValue)
        {
            this.baseValue = baseValue;
        }

        /// <summary>Inspector orqali beriladigan boshlang'ich (bazaviy) qiymat.</summary>
        public float BaseValue
        {
            get => baseValue;
            set
            {
                baseValue = value;
                isDirty = true;
            }
        }

        /// <summary>Barcha modifikatorlar hisobga olingan yakuniy qiymat.</summary>
        public float Value
        {
            get
            {
                if (isDirty)
                {
                    Recalculate();
                }
                return cachedValue;
            }
        }

        /// <summary>
        /// Berilgan manba (masalan skillId yoki buffId) uchun modifikator o'rnatadi
        /// yoki mavjudini yangilaydi.
        /// </summary>
        public void SetModifier(string sourceId, float flat = 0f, float percent = 0f)
        {
            modifiers[sourceId] = new StatModifier(sourceId, flat, percent);
            isDirty = true;
        }

        /// <summary>Berilgan manbadan qo'yilgan modifikatorni olib tashlaydi.</summary>
        public void RemoveModifier(string sourceId)
        {
            if (modifiers.Remove(sourceId))
            {
                isDirty = true;
            }
        }

        /// <summary>Barcha modifikatorlarni tozalaydi (masalan statni reset qilishda).</summary>
        public void ClearModifiers()
        {
            if (modifiers.Count == 0) return;
            modifiers.Clear();
            isDirty = true;
        }

        private void Recalculate()
        {
            float flatSum = 0f;
            float percentSum = 0f;

            foreach (var modifier in modifiers.Values)
            {
                Debug.Log(
                    $"[ModifiableStat] " +
                    $"Source='{modifier.SourceId}' " +
                    $"Flat={modifier.Flat} " +
                    $"Percent={modifier.Percent}"
                );

                flatSum += modifier.Flat;
                percentSum += modifier.Percent;
            }

            cachedValue = (baseValue + flatSum) * (1f + percentSum);

            Debug.Log(
                $"[ModifiableStat] " +
                $"Base={baseValue}, " +
                $"FlatSum={flatSum}, " +
                $"PercentSum={percentSum}, " +
                $"RESULT={cachedValue}"
            );

            isDirty = false;
        }

        /// <summary>
        /// Qulaylik uchun: ModifiableStat'ni to'g'ridan-to'g'ri float sifatida ham
        /// ishlatish mumkin (masalan Debug.Log($"{stat}") kabi holatlarda).
        /// Skript kodida esa aniqlik uchun har doim `.Value` ishlatish tavsiya etiladi.
        /// </summary>
        public static implicit operator float(ModifiableStat stat) => stat?.Value ?? 0f;
    }
}