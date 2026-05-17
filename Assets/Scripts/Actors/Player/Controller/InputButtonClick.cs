using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Actors.Player.Controller
{
    public class InputButtonClick : InputButtonBase
    {
        public UnityEvent<bool> onClick;
        
        public override void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke(true);
        }
    }
}
