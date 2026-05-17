using Core.Initialization;
using UI.Widgets;

namespace UI.Menu.Panels
{
    public class MenuPanel: Panel
    {
        protected override void OnPanelShow()
        {
            Hub.sceneManager.StopSceneTime();
        }

        protected override void OnPanelHide()
        {
            Hub.sceneManager.StartSceneTime();
        }
    }
}