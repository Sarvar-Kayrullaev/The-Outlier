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
        
        // Yangi birlashtirilgan o'zgaruvchilar
        private SerializedProperty minValue;
        private SerializedProperty maxValue;
        private SerializedProperty value;

        private SerializedProperty fillerColor;
        private SerializedProperty fillerHoverColor;
        private SerializedProperty animationEnabled;
        private SerializedProperty fillerTargetHeightScale;
        private SerializedProperty animatingTime;

        // Yangi info/tooltip property-lari
        private SerializedProperty infoPrefab;
        private SerializedProperty infoYOffset;

        private SerializedProperty containerRect;
        private SerializedProperty fillerRect;
        private SerializedProperty fillerImage;
        private SerializedProperty valueText;
        private SerializedProperty onValueChanged;

        private void OnEnable()
        {
            // Property-larni yangi o'zgaruvchilarga qarab bog'lab olamiz
            interactable = serializedObject.FindProperty("interactable");
            isWholeNumber = serializedObject.FindProperty("isWholeNumber");
        
            minValue = serializedObject.FindProperty("minValue");
            maxValue = serializedObject.FindProperty("maxValue");
            value = serializedObject.FindProperty("value");
            
            fillerColor = serializedObject.FindProperty("fillerColor");
            fillerHoverColor = serializedObject.FindProperty("fillerHoverColor");

            animationEnabled = serializedObject.FindProperty("animation");
            fillerTargetHeightScale = serializedObject.FindProperty("fillerTargetHeightScale");
            animatingTime = serializedObject.FindProperty("animatingTime");

            infoPrefab = serializedObject.FindProperty("infoPrefab");
            infoYOffset = serializedObject.FindProperty("infoYOffset");

            containerRect = serializedObject.FindProperty("containerRect");
            fillerRect = serializedObject.FindProperty("fillerRect");
            fillerImage = serializedObject.FindProperty("fillerImage");
            valueText = serializedObject.FindProperty("valueText");
            
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
                EditorGUILayout.PropertyField(minValue, new GUIContent("Min Value"));
                EditorGUILayout.PropertyField(maxValue, new GUIContent("Max Value"));
            
                // Float Slider ko'rinishida joriy qiymat
                value.floatValue = EditorGUILayout.Slider(
                    new GUIContent("Current Value"), 
                    value.floatValue, 
                    minValue.floatValue, 
                    maxValue.floatValue
                );
            }
            else
            {
                // Int sozlamalari (Faqat butun sonlar kiritilishini ta'minlaymiz)
                int minInt = Mathf.RoundToInt(minValue.floatValue);
                int maxInt = Mathf.RoundToInt(maxValue.floatValue);

                EditorGUI.BeginChangeCheck();
                minInt = EditorGUILayout.IntField(new GUIContent("Min Value"), minInt);
                maxInt = EditorGUILayout.IntField(new GUIContent("Max Value"), maxInt);
                if (EditorGUI.EndChangeCheck())
                {
                    minValue.floatValue = minInt;
                    maxValue.floatValue = maxInt;
                }
            
                // Int Slider ko'rinishida joriy qiymat
                int currentInt = Mathf.RoundToInt(value.floatValue);
                currentInt = EditorGUILayout.IntSlider(
                    new GUIContent("Current Value"), 
                    currentInt, 
                    minInt, 
                    maxInt
                );
                value.floatValue = currentInt;
            }
            EditorGUILayout.EndVertical();

            // YANGI: Tooltip / Info Sozlamalari bo'limi
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Tooltip / Info Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(infoPrefab, new GUIContent("Info Prefab"));
            EditorGUILayout.PropertyField(infoYOffset, new GUIContent("Y Offset"));
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
            EditorGUILayout.PropertyField(valueText);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(onValueChanged);

            // O'zgarishlarni saqlash
            serializedObject.ApplyModifiedProperties();

            // Agar Editor-da biror narsa o'zgarsa, vizual yangilanish uchun sahna va obyektni "Dirty" (o'zgargan) deb belgilaymiz
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}