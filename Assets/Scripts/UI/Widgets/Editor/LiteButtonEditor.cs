using UnityEditor;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(LiteButton))]
    [CanEditMultipleObjects]
    public class LiteButtonEditor : UnityEditor.Editor 
    {
        #region Serialized Properties

        private SerializedProperty primaryColor;
        private SerializedProperty hoverPrimaryColor;
        private SerializedProperty isSecondaryColor;
        private SerializedProperty secondaryColor;
        private SerializedProperty hoverSecondaryColor;
        private SerializedProperty onPointerDown;
        private SerializedProperty onPointerUp;
        private SerializedProperty onClick;
        #endregion

        private void OnEnable()
        {
            primaryColor = serializedObject.FindProperty("primaryColor");
            hoverPrimaryColor = serializedObject.FindProperty("hoverPrimaryColor");
            isSecondaryColor = serializedObject.FindProperty("isSecondaryColor");
            secondaryColor = serializedObject.FindProperty("secondaryColor");
            hoverSecondaryColor = serializedObject.FindProperty("hoverSecondaryColor");
            onPointerDown = serializedObject.FindProperty("onPointerDown");
            onPointerUp = serializedObject.FindProperty("onPointerUp");
            onClick = serializedObject.FindProperty("onClick");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("General Configuration", EditorStyles.boldLabel);
            
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(primaryColor);
                EditorGUILayout.PropertyField(hoverPrimaryColor);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(isSecondaryColor);
                if (isSecondaryColor.boolValue) 
                {
                    EditorGUILayout.PropertyField(secondaryColor);
                    EditorGUILayout.PropertyField(hoverSecondaryColor);
                }
            }
            
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(onPointerDown);
            EditorGUILayout.PropertyField(onPointerUp);
            EditorGUILayout.PropertyField(onClick);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}