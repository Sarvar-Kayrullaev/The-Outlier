using UnityEngine;

namespace UI.Widgets
{
    public abstract class TabFragmentView : MonoBehaviour
    {
        public virtual void OnFragmentInit() { }

        public virtual void OnFragmentSelected() { }

        public virtual void OnFragmentDeactivated() { }
    }
}