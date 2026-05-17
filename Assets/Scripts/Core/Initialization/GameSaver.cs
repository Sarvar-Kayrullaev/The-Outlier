using System;
using Actors.Player.Core;
using Core.Services;
using Data.Models.World;
using Interfaces;
using UnityEngine;

namespace Core.Initialization
{
    public class GameSaver : MonoBehaviour, ISingle
    {
        public void SaveGame()
        {
            var playerManager = Hub.playerInitializer.playerManager;
            var worldModel = Hub.dataService.GetWorldData();
            worldModel.player.position = playerManager.transform.position;
            worldModel.player.rotationAngle =  playerManager.transform.rotation.eulerAngles.y;
            Hub.dataService.SaveCurrentData();
        }
        
        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                SaveGame();
            }
        }
    }
}