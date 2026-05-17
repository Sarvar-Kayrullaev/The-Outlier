using UnityEngine;
using UnityEngine.EventSystems;

namespace Actors.Player.Controller
{
    public abstract class InputButtonBase: MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IPointerExitHandler, IDragHandler, IPointerClickHandler
    {
        public virtual void OnPointerDown(PointerEventData eventData) {}

        public virtual void OnPointerUp(PointerEventData eventData)  {}

        public virtual void OnPointerExit(PointerEventData eventData)  {}

        public virtual void OnDrag(PointerEventData eventData)  {}
        
        public virtual void OnPointerClick(PointerEventData eventData) { }
    }
}