using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Actors.Player.Controller
{
    public class InputButtonHit : InputButtonBase
    {
        public UnityEvent<bool> onHit;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            onHit.Invoke(true);
        }
    }
}
