using Actors.Player.Core;
using Interfaces;
using UnityEngine;

namespace Core.Initialization
{
    public class PlayerInitializer: MonoBehaviour, ISingle
    {
        [HideInInspector] public PlayerManager playerManager;
        [Header("References")]
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private AudioSource audioSource;
        
        private GameObject playerGameObject;

        public void Initialize()
        {
            var playerModel = Hub.dataService.GetWorldData().player;
            playerGameObject = Instantiate(playerPrefab);
            playerGameObject.transform.position = playerModel.position;
            playerGameObject.transform.rotation = Quaternion.Euler(0, playerModel.rotationAngle, 0);
            if (playerGameObject.TryGetComponent(out playerManager))
            {
                Hub.playerManager = playerManager;
                playerManager.audioSource = audioSource;
            }
        }
    }
}