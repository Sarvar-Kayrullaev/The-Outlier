using UI.Widgets;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Menu.Fragments.Main
{
    public class MainFragmentView : TabFragmentView
    {
        [Header("ViewModel Connection")]
        [SerializeField] private MainFragmentViewModel viewModel;

        [Header("Buttons")]
        [SerializeField] private LiteButton continueLiteButton;
        [SerializeField] private LiteButton restartMissionLiteButton;
        [SerializeField] private LiteButton abandonMissionLiteButton;
        [SerializeField] private LiteButton settingsLiteButton;
        [SerializeField] private LiteButton saveGameLiteButton;
        [SerializeField] private LiteButton exitLiteButton;

        private void Awake()
        {
            continueLiteButton.onClick.AddListener(viewModel.ContinueGame);
            restartMissionLiteButton.onClick.AddListener(viewModel.RestartMission);
            abandonMissionLiteButton.onClick.AddListener(viewModel.AbandonMission);
            settingsLiteButton.onClick.AddListener(viewModel.OpenSettings);
            saveGameLiteButton.onClick.AddListener(viewModel.SaveGame);
            exitLiteButton.onClick.AddListener(viewModel.ExitToMainMenu);
        }
        
        public override void OnFragmentInit()
        {
            
        }

        private void OnEnable()
        {
            RefreshLayout();
        }

        private void RefreshLayout()
        {
            var hasActiveMission = viewModel.IsMissionActive();

            // Only show Restart/Abandon if a mission is in progress
            restartMissionLiteButton.gameObject.SetActive(hasActiveMission);
            abandonMissionLiteButton.gameObject.SetActive(hasActiveMission);
        }
    }
}