#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(TabView))]
    public class TabViewEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            TabView tabView = (TabView)target;

            if (GUILayout.Button("Refresh Tabs and Fragments"))
            {
                Undo.RecordObject(tabView, "Refresh TabView");

                // Auto assign buttons from tabParent
                tabView.tabButtonViews.Clear();
                if (tabView.tabParent != null)
                {
                    tabView.tabButtonViews.AddRange(tabView.tabParent.GetComponentsInChildren<TabButtonView>(true));
                }

                // Auto assign fragments from fragmentParent
                tabView.fragmentViews.Clear();
                if (tabView.fragmentParent != null)
                {
                    // Only get direct children to avoid nested fragments
                    foreach (Transform child in tabView.fragmentParent)
                    {
                        var fragment = child.GetComponent<TabFragmentView>();
                        if (fragment != null) tabView.fragmentViews.Add(fragment);
                    }
                }

                EditorUtility.SetDirty(tabView);
                Debug.Log($"TabView Refreshed: {tabView.tabButtonViews.Count} tabs and {tabView.fragmentViews.Count} fragments found.");
            }
        }
    }
}
#endif