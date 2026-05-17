using UnityEngine;

namespace UI.Widgets
{
    public abstract class Panel : MonoBehaviour
    {
        public void Show()
        {
            gameObject.SetActive(true);
            OnPanelShow();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            OnPanelHide();
        }
        
        protected abstract void OnPanelShow();

        protected abstract void OnPanelHide();
    }
}