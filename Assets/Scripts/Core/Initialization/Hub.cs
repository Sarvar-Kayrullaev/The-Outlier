
using Actors.Player.Core;
using Core.Managers;
using Core.Services;
using UI.Widgets;
using SceneManager = Core.Managers.SceneManager;

// ReSharper disable UnassignedField.Global
namespace Core.Initialization
{
    public static class Hub {
        
        public static DataService dataService;
        public static GameInitializer gameInitializer;
        public static PlayerInitializer playerInitializer;
        public static GameSaver gameSaver;
        public static MissionManager missionManager;
        public static SceneManager sceneManager;
        public static LocalizationService localizationService;
        public static PanelManager panelManager;
        public static HUDManager hudManager;
        public static SkillManager skillManager;
        public static FundManager fundManager;
        
        // Later
        public static PlayerManager playerManager;
    }
}