using Actors.Player.Core;
using Actors.Player.Scriptable;
using Interfaces;

namespace Actors.Player.Modules
{
    public class SwimmingState : IState
    {
        private ActionController _controller;
        private PlayerStats _stats;

        public SwimmingState(ActionController controller)
        {
            _controller = controller;
            _stats = controller.stats;
        }

        public void Enter()
        {
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
        }
    }
}