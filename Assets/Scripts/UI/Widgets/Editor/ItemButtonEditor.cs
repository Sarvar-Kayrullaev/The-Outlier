using UnityEditor;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(ItemButton))]
    [CanEditMultipleObjects]
    public class ItemButtonEditor : UnityEditor.Editor 
    {
        #region Serialized Properties

        private SerializedProperty selectedButtonColor;
        private SerializedProperty unselectedButtonColor;
        private SerializedProperty buttonHoverColor;
        private SerializedProperty isSecondaryColor;
        private SerializedProperty selectedSecondaryColor;
        private SerializedProperty unselectedSecondaryColor;
        private SerializedProperty hoverSecondaryColor;
        private SerializedProperty onPointerDown;
        private SerializedProperty onPointerUp;
        private SerializedProperty onClick;

        #endregion

        private void OnEnable()
        {
            selectedButtonColor = serializedObject.FindProperty("selectedPrimaryColor");
            unselectedButtonColor = serializedObject.FindProperty("unselectedPrimaryColor");
            buttonHoverColor = serializedObject.FindProperty("hoverPrimaryColor");
            isSecondaryColor = serializedObject.FindProperty("isSecondaryColor");
            selectedSecondaryColor = serializedObject.FindProperty("selectedSecondaryColor");
            unselectedSecondaryColor = serializedObject.FindProperty("unselectedSecondaryColor");
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
                EditorGUILayout.PropertyField(selectedButtonColor);
                EditorGUILayout.PropertyField(unselectedButtonColor);
                EditorGUILayout.PropertyField(buttonHoverColor);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(isSecondaryColor);
                if (isSecondaryColor.boolValue) 
                {
                    EditorGUILayout.PropertyField(selectedSecondaryColor);
                    EditorGUILayout.PropertyField(unselectedSecondaryColor);
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