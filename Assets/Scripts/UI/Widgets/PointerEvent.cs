using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Widgets
{
    public class PointerEvent: MonoBehaviour , IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public UnityEvent onPointerDown = new();
        public UnityEvent onPointerUp = new();
        public UnityEvent onClick = new();
        
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClick.Invoke();
        }
    }
}