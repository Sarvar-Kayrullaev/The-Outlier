using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Widgets
{
    public class TabButtonView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public TMP_Text text;
        public bool selected = false;
        [Space(3)]
        public UnityEvent OnClick;
        public UnityEvent OnDown;
        public UnityEvent OnUp;

        public void SetSelection(bool selected, Color color)
        {
            this.selected = selected;
            text.color = color;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
        }
    }
}