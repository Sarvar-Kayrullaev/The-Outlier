using UnityEngine;

namespace UI.Widgets
{
    public class TabFragmentView : MonoBehaviour
    {
        public virtual void OnFragmentInit() { }

        public virtual void OnFragmentSelected() { }

        public virtual void OnFragmentDeactivated() { }
    }
}