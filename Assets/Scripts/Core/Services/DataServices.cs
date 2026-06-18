using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models.Core;
using Data.Models.World;
using Handlers;
using Interfaces;
using UnityEngine;

namespace Core.Services
{
    public class DataService : MonoBehaviour, ISingle
    {
        private const string gameDataFile = "game_data.json";
        private const string settingsDataFileName = "settings_data.json";
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
            cashedGameData = FileHandler.Exists(gameDataFile) ? FileHandler.ReadFromJSON<GameData>(gameDataFile) : new GameData();
            return cashedGameData;
        }

        public WorldModel GetWorldData()
        {
            if(cashedWorldModel != null) return cashedWorldModel;
            var worldId = GetGameData().lastSelectedWorldID;
            cashedWorldModel = LoadWorld(worldId);
            return cashedWorldModel;
        }
        
        public SettingsData GetSettingsData()
        {
            if (cashedSettingsData != null) return cashedSettingsData;
            cashedSettingsData = FileHandler.Exists(settingsDataFileName) ? FileHandler.ReadFromJSON<SettingsData>(settingsDataFileName) : new SettingsData();
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

            // If the world is not found, we create a new one and add it to the list
            Debug.Log($"World {worldId} not found. Creating a new world...");
            var newWorld = CreateNewWorld(worldId);
            gameData.worlds.Add(newWorld);
            gameData.lastSelectedWorldID = worldId;
            SaveAll(); // We save the new world along with the list
            
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

        // 3. Update the world (Save)
        public void UpdateWorldData(WorldModel updatedPlayerModel)
        {
            var gameData = GetGameData();
            
            // Find the old data in the list
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

        private static WorldModel CreateNewWorld(int id)
        {
            return new WorldModel
            {
                worldID = id,
                funds = new FundModel { Balance = 100, SkillPoints = 0 },
                abilities = new List<PlayerAbilityModel>(),
                outposts = new List<OutpostModel>()
            };
        }
    }
}