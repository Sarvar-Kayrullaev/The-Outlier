using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models.Core;
using Data.Models.World;
using Data.Templates.Player;
using Handlers;
using Interfaces;
using UnityEngine;

namespace Core.Services
{
    public class DataService : MonoBehaviour, ISingle
    {
        private const string gameDataFile = "game_data.json";
        private const string settingsDataFileName = "settings_data.json";

        [Header("Starter Configuration")] [SerializeField]
        private InitialWorldConfig initialWorldConfig; // Inspektor orqali ScriptableObject assetini biriktirasiz

        private GameData cashedGameData;
        private SettingsData cashedSettingsData;
        private WorldModel cashedWorldModel;

        private void Awake()
        {
            GetSettingsData();
            GetGameData();
            GetWorldData();
        }

        public GameData GetGameData()
        {
            if (cashedGameData != null) return cashedGameData;
            cashedGameData = FileHandler.Exists(gameDataFile)
                ? FileHandler.ReadFromJSON<GameData>(gameDataFile)
                : new GameData();
            return cashedGameData;
        }

        public WorldModel GetWorldData()
        {
            if (cashedWorldModel != null) return cashedWorldModel;
            var worldId = GetGameData().lastSelectedWorldID;
            cashedWorldModel = LoadWorld(worldId);
            return cashedWorldModel;
        }

        public SettingsData GetSettingsData()
        {
            if (cashedSettingsData != null) return cashedSettingsData;
            cashedSettingsData = FileHandler.Exists(settingsDataFileName)
                ? FileHandler.ReadFromJSON<SettingsData>(settingsDataFileName)
                : new SettingsData();
            return cashedSettingsData;
        }

        public WorldModel LoadWorld(int worldId)
        {
            var gameData = GetGameData();
            var world = gameData.worlds.FirstOrDefault(w => w.worldID == worldId);
            if (world != null)
            {
                gameData.lastSelectedWorldID = worldId;
                return world;
            }

            Debug.Log($"World {worldId} not found. Creating a new world using config...");
            // ScriptableObject asosida yangi dunyo yaratish
            var newWorld = CreateNewWorld(worldId);
            gameData.worlds.Add(newWorld);
            gameData.lastSelectedWorldID = worldId;
            SaveAll();

            return newWorld;
        }

        public void SaveCurrentGameData()
        {
            if (cashedGameData == null || cashedWorldModel == null) return;

            var index = cashedGameData.worlds.FindIndex(w => w.worldID == cashedWorldModel.worldID);
            if (index != -1)
            {
                cashedGameData.worlds[index] = cashedWorldModel;
            }

            SaveAll();
        }

        public void UpdateWorldData(WorldModel updatedPlayerModel)
        {
            var gameData = GetGameData();
            var index = gameData.worlds.FindIndex(w => w.worldID == updatedPlayerModel.worldID);

            if (index != -1)
            {
                gameData.worlds[index] = updatedPlayerModel;
            }
            else
            {
                gameData.worlds.Add(updatedPlayerModel);
            }

            SaveAll();
        }

        public void SaveSettingsData()
        {
            if (cashedSettingsData == null) return;
            FileHandler.SaveToJSON(cashedSettingsData, settingsDataFileName);
            Debug.Log("Settings Saved!");
        }

        private void SaveAll()
        {
            if (cashedGameData == null) return;
            FileHandler.SaveToJSON(cashedGameData, gameDataFile);
            Debug.Log("Save!");
        }

        // ScriptableObject ma'lumotlari bilan to'ldiriladigan yangi metod
        private WorldModel CreateNewWorld(int id)
        {
            // 1. Agar inspektorda config biriktirilmagan bo'lsa, xavfsizlik chorasi (Fallback)
            if (initialWorldConfig == null)
            {
                Debug.LogWarning("InitialWorldConfig is missing in DataService! Using fallback hardcoded data.");
                return new WorldModel
                {
                    worldID = id,
                    player = new PlayerModel { position = Vector3.zero, rotationAngle = 0f },
                    funds = new FundModel { Balance = 100, SkillPoints = 0 },
                    skills = new List<PlayerSkillModel>(), // Bo'sh qaytadi
                    outposts = new List<OutpostModel>()
                };
            }

            // 2. Outpostlar ro'yxatini config faylidan aylanib chiqib generatsiya qilish
            List<OutpostModel> starterOutposts = new List<OutpostModel>();
            if (initialWorldConfig.defaultOutpostIds != null)
            {
                foreach (var outpostId in initialWorldConfig.defaultOutpostIds)
                {
                    starterOutposts.Add(new OutpostModel { Id = outpostId, Captured = false });
                }
            }

            // 3. SKILLARNI TO'LDIRISH (Siz so'ragan va tushunarsiz bo'lgan asosiy qism)
            List<PlayerSkillModel> starterSkills = new List<PlayerSkillModel>();

            // Config ichidagi barcha skill ScriptableObject'larini aylanib chiqamiz
            if (initialWorldConfig.allGameSkills != null)
            {
                foreach (var skillTemplate in initialWorldConfig.allGameSkills)
                {
                    if (skillTemplate != null)
                    {
                        // Har bir skill uchun yangi saqlash modeli yaratamiz
                        // ID sini ScriptableObject dan oladi, 'unlocked' holatini esa false (locked) qiladi
                        PlayerSkillModel newLockedSkill = new PlayerSkillModel
                        {
                            id = skillTemplate.id,
                            unlocked = false
                        };

                        // Ro'yxatga qo'shamiz
                        starterSkills.Add(newLockedSkill);
                    }
                }
            }

            // 4. To'liq tayyorlangan modellarni yangi yaratilgan WorldModel ga yuklab qaytaramiz
            return new WorldModel
            {
                worldID = id,
                player = new PlayerModel
                {
                    position = initialWorldConfig.startPosition,
                    rotationAngle = initialWorldConfig.startRotationAngle
                },
                funds = new FundModel
                {
                    Balance = initialWorldConfig.startBalance,
                    SkillPoints = initialWorldConfig.startSkillPoints
                },
                skills = starterSkills, // Mana endi bu yerda bo'sh ro'yxat emas, hammasi LOCKED bo'lgan ro'yxat boradi!
                outposts = starterOutposts
            };
        }
    }
}