using UnityEditor;
using UnityEngine;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(SeekBar))]
    [CanEditMultipleObjects]
    public class SeekBarEditor : UnityEditor.Editor
    {
        private SerializedProperty interactable;
        private SerializedProperty isWholeNumber;
        private SerializedProperty minFloatValue, maxFloatValue, floatValue;
        private SerializedProperty minIntValue, maxIntValue, intValue;
        private SerializedProperty fillerColor;
        private SerializedProperty fillerHoverColor;
        private SerializedProperty animationEnabled;
        private SerializedProperty fillerTargetHeightScale;
        private SerializedProperty animatingTime;
        private SerializedProperty containerRect;
        private SerializedProperty fillerRect;
        private SerializedProperty fillerImage;
        private SerializedProperty onValueChanged;

        private void OnEnable()
        {
            // Property-larni bog'lab olamiz
            interactable = serializedObject.FindProperty("interactable");
            isWholeNumber = serializedObject.FindProperty("isWholeNumber");
        
            minFloatValue = serializedObject.FindProperty("minFloatValue");
            maxFloatValue = serializedObject.FindProperty("maxFloatValue");
            floatValue = serializedObject.FindProperty("floatValue");

            minIntValue = serializedObject.FindProperty("minIntValue");
            maxIntValue = serializedObject.FindProperty("maxIntValue");
            intValue = serializedObject.FindProperty("intValue");
            
            fillerColor = serializedObject.FindProperty("fillerColor");
            fillerHoverColor = serializedObject.FindProperty("fillerHoverColor");

            animationEnabled = serializedObject.FindProperty("animation");
            fillerTargetHeightScale = serializedObject.FindProperty("fillerTargetHeightScale");
            animatingTime = serializedObject.FindProperty("animatingTime");

            containerRect = serializedObject.FindProperty("containerRect");
            fillerRect = serializedObject.FindProperty("fillerRect");
            fillerImage = serializedObject.FindProperty("fillerImage");
            
            onValueChanged = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Main Settings", EditorStyles.boldLabel);
        
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(interactable);
            EditorGUILayout.PropertyField(isWholeNumber, new GUIContent("Is Whole Number (Integer)"));
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Value Configuration", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            if (!isWholeNumber.boolValue)
            {
                // Float sozlamalari
                EditorGUILayout.PropertyField(minFloatValue, new GUIContent("Min Value"));
                EditorGUILayout.PropertyField(maxFloatValue, new GUIContent("Max Value"));
            
                // Slider ko'rinishida joriy qiymat
                floatValue.floatValue = EditorGUILayout.Slider(
                    new GUIContent("Current Value"), 
                    floatValue.floatValue, 
                    minFloatValue.floatValue, 
                    maxFloatValue.floatValue
                );
            }
            else
            {
                // Int sozlamalari
                EditorGUILayout.PropertyField(minIntValue, new GUIContent("Min Value"));
                EditorGUILayout.PropertyField(maxIntValue, new GUIContent("Max Value"));
            
                // Slider ko'rinishida joriy qiymat
                intValue.intValue = EditorGUILayout.IntSlider(
                    new GUIContent("Current Value"), 
                    intValue.intValue, 
                    minIntValue.intValue, 
                    maxIntValue.intValue
                );
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Visual & Animation", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(fillerColor);
            EditorGUILayout.PropertyField(fillerHoverColor);
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(animationEnabled, new GUIContent("Enable Animation"));
        
            if (animationEnabled.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(fillerTargetHeightScale, new GUIContent("Hover Scale Y"));
                EditorGUILayout.PropertyField(animatingTime, new GUIContent("Duration"));
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(containerRect);
            EditorGUILayout.PropertyField(fillerRect);
            EditorGUILayout.PropertyField(fillerImage);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(onValueChanged);

            // O'zgarishlarni saqlash
            serializedObject.ApplyModifiedProperties();

            // Agar Editor-da biror narsa o'zgarsa, SeekBar-dagi OnValidate-ni chaqirish (vizual yangilanish uchun)
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}