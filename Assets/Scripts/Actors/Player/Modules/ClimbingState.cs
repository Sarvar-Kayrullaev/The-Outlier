using Actors.Player.Core;
using Actors.Player.Scriptable;

namespace Actors.Player.Modules
{
    public class ClimbingState
    {
        private ActionController _controller;
        private PlayerStats _stats;

        public ClimbingState(ActionController controller)
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