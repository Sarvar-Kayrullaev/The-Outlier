using System.Collections.Generic;
using Core.Initialization;
using Data.Models.World;
using Data.Templates.Player;
using Interfaces;
using UnityEngine;

namespace Core.Managers
{
    public class SkillManager : MonoBehaviour, ISingle
    {
        [Header("Database")]
        // DIQQAT: SkillNode (yoki AbilityNode) ichidagi 'id' ham string bo'lishi kerak
        [SerializeField] private List<SkillNode> allSkills;
    
        // Qobiliyat ID si (string) bo'yicha ochilgan/ochilmaganligini saqlash
        private Dictionary<string, bool> unlockedSkills = new Dictionary<string, bool>();

        private void Start()
        {
            LoadSkillTreeData();
        }

        // Loyihadagi ma'lumotlar bazasidan yuklash
        public void LoadSkillTreeData()
        {
            unlockedSkills.Clear();
            var worldData = Hub.dataService.GetWorldData();
    
            // 1. Agar saqlangan faylda skillar bo'lsa, ularni dictionary'ga yuklaymiz
            if (worldData != null && worldData.skills != null)
            {
                foreach (var skill in worldData.skills)
                {
                    unlockedSkills[skill.id] = skill.unlocked;
                }
            }

            // 2. DIQQAT: Agarda datada ba'zi skillar umuman mavjud bo'lmasa (masalan, yangi o'yin boshlanganda 0 ta bo'lsa)
            // yoki keyinchalik o'yinga yangi skill qo'shgan bo'lsangiz, ularni avtomatik locked (false) qilib qo'shamiz
            bool needToSaveFallback = false;
    
            foreach (var skill in allSkills)
            {
                if (skill == null) continue;

                if (!unlockedSkills.ContainsKey(skill.id))
                {
                    unlockedSkills[skill.id] = false; // Barchasini locked holatda yaratamiz
                    needToSaveFallback = true;         // Yangi skill qo'shilgani uchun faylni yangilash kerakligini belgilaymiz
                }
            }

            // 3. Agar ro'yxat to'ldirilgan bo'lsa, uni birdan faylga ham saqlab qo'yamiz (keyingi safar bo'sh kelmasligi uchun)
            if (needToSaveFallback)
            {
                SaveSkillTreeData();
            }
        }

        // ID turi string qilib o'zgartirildi
        public bool IsSkillUnlocked(string id)
        {
            return !string.IsNullOrEmpty(id) && unlockedSkills.ContainsKey(id) && unlockedSkills[id];
        }

        // Qobiliyatni sotib olish/ochish funksiyasi
        public bool TryUnlockSkill(SkillNode skill)
        {
            if (skill == null) return false;
            if (IsSkillUnlocked(skill.id)) return false; // Allaqachon ochilgan

            // Oldingi shart (Parent) bajarilganmi tekshirish
            if (skill.parentNode != null && !IsSkillUnlocked(skill.parentNode.id))
            {
                Debug.LogWarning("Oldin bog'langan qobiliyatni ochish kerak!");
                return false;
            }

            var fundManager = Hub.fundManager;
            if (fundManager != null && fundManager.SpendSkillPoint(skill.cost))
            {
                unlockedSkills[skill.id] = true;
                SaveSkillTreeData();
                return true;
            }

            Debug.LogWarning("Skill point yetarli emas!");
            return false;
        }

        private void SaveSkillTreeData()
        {
            var worldData = Hub.dataService.GetWorldData();
            if (worldData != null)
            {
                worldData.skills = new List<PlayerSkillModel>();
                foreach (var kvp in unlockedSkills)
                {
                    worldData.skills.Add(new PlayerSkillModel { id = kvp.Key, unlocked = kvp.Value });
                }
                Hub.dataService.SaveCurrentGameData();
            }
        }
    }
}