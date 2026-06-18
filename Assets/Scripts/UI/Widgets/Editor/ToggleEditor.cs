using UnityEditor;
using UnityEngine;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(Toggle))]
    [CanEditMultipleObjects]
    public class ToggleEditor : UnityEditor.Editor
    {
        private SerializedProperty interactable;
        private SerializedProperty isOn;

        private SerializedProperty toggleRect;
        private SerializedProperty handlerRect;
        private SerializedProperty backgroundImage;
        private SerializedProperty handlerImage;

        private SerializedProperty bgTrueColor;
        private SerializedProperty bgFalseColor;
        private SerializedProperty fgTrueColor;
        private SerializedProperty fgFalseColor;

        // Renamed properties from Hover to Press to match the target script
        private SerializedProperty bgPressColor;
        private SerializedProperty fgPressColor;

        private SerializedProperty onValueChanged;

        private void OnEnable()
        {
            // Caching serialized properties from the Toggle component
            interactable = serializedObject.FindProperty("interactable");
            isOn = serializedObject.FindProperty("isOn");

            toggleRect = serializedObject.FindProperty("toggleRect");
            handlerRect = serializedObject.FindProperty("handlerRect");
            backgroundImage = serializedObject.FindProperty("backgroundImage");
            handlerImage = serializedObject.FindProperty("handlerImage");

            bgTrueColor = serializedObject.FindProperty("bgTrueColor");
            bgFalseColor = serializedObject.FindProperty("bgFalseColor");
            fgTrueColor = serializedObject.FindProperty("fgTrueColor");
            fgFalseColor = serializedObject.FindProperty("fgFalseColor");

            // Linked to the new variable names in the target script
            bgPressColor = serializedObject.FindProperty("bgPressColor");
            fgPressColor = serializedObject.FindProperty("fgPressColor");

            onValueChanged = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            // Fetch current values from the real component
            serializedObject.Update();

            // SECTION 1: Main Toggle Settings
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Main Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(interactable);
            EditorGUILayout.PropertyField(isOn, new GUIContent("Is On"));
            EditorGUILayout.EndVertical();

            // SECTION 2: UI Component References
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("UI References", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(toggleRect, new GUIContent("Toggle RectTransform"));
            EditorGUILayout.PropertyField(handlerRect, new GUIContent("Handler RectTransform"));
            EditorGUILayout.PropertyField(backgroundImage, new GUIContent("Background Image"));
            EditorGUILayout.PropertyField(handlerImage, new GUIContent("Handler Image"));
            EditorGUILayout.EndVertical();

            // SECTION 3: Main State Colors (True / False)
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Colors", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.LabelField("Background", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(bgTrueColor, new GUIContent("  True"));
            EditorGUILayout.PropertyField(bgFalseColor, new GUIContent("  False"));
            
            EditorGUILayout.Space(3);
            
            EditorGUILayout.LabelField("Handler", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(fgTrueColor, new GUIContent("  True"));
            EditorGUILayout.PropertyField(fgFalseColor, new GUIContent("  False"));

            // SECTION 4: Touch/Press Colors (Replaced Hover section)
            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField("Touch Color", EditorStyles.miniBoldLabel);
            EditorGUILayout.PropertyField(bgPressColor, new GUIContent("Background Press"));
            EditorGUILayout.PropertyField(fgPressColor, new GUIContent("Handler Press"));
            EditorGUILayout.EndVertical();

            // SECTION 5: Unity Events
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(onValueChanged);

            // Apply any modifications made in the inspector to the actual object data
            serializedObject.ApplyModifiedProperties();

            // Marks the target object as 'dirty' so Unity knows it needs to save the changes
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}