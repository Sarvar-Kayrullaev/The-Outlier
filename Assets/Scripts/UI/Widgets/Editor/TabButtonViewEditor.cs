using TMPro;
using UnityEditor;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(TabButtonView))]
    public class TabButtonViewEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var tabButtonView = (TabButtonView)target;
            if (tabButtonView.text != null)
            {
                if (tabButtonView.transform.childCount > 0)
                {
                    tabButtonView.text = tabButtonView.GetComponentInChildren<TMP_Text>();
                }
            }
            
            EditorUtility.SetDirty(tabButtonView);
        }
    }
}