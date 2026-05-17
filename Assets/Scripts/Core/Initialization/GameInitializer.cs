using System;
using Actors.Player.Core;
using Core.Services;
using Interfaces;
using UnityEngine;

namespace Core.Initialization
{
    public class GameInitializer: MonoBehaviour, ISingle
    {
        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            Hub.playerInitializer.Initialize();
        }
    }
}