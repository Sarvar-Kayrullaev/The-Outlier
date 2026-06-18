using Core.Initialization;
using UnityEngine;

namespace UI.Menu.Fragments.Main
{
    public class MainFragmentViewModel : MonoBehaviour
    {

        public void ContinueGame()
        {
            Hub.panelManager.CloseCurrentPanel();
        }

        public void RestartMission()
        {
            
        }

        public void AbandonMission()
        {
            if (Hub.missionManager != null)
            {
                Hub.missionManager.EndMission();
            }
            ContinueGame();
        }

        public void OpenSettings()
        {
            //Hub.panelManager.OpenPanelByName("SettingsPanel");
        }

        public void SaveGame()
        {
            Hub.gameSaver.SaveGame();
        }

        public void ExitToMainMenu()
        {
            Hub.sceneManager.ExitToMainMenu();
        }

        public bool IsMissionActive()
        {
            return Hub.missionManager.IsMissionActive;
        }
    }
}