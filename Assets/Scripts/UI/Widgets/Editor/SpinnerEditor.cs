using UnityEditor;

namespace UI.Widgets.Editor
{
    [CustomEditor(typeof(Spinner))]
    [CanEditMultipleObjects]
    public class SpinnerEditor : UnityEditor.Editor
    {
        #region Serialized Properties
        private SerializedProperty _spinnerType;
        private SerializedProperty _interactable;
        private SerializedProperty _animationDuration;

        private SerializedProperty _splitToRows;
        private SerializedProperty _rowSize;
        private SerializedProperty _rowSpacing;
        private SerializedProperty _padding;
        private SerializedProperty _itemHeight;
        private SerializedProperty _itemWidth;
        private SerializedProperty _itemSpacing;
        private SerializedProperty _selectedItemColor;
        private SerializedProperty _unselectedItemColor;
        private SerializedProperty _selectedSecondaryColor;
        private SerializedProperty _unselectedSecondaryColor;

        private SerializedProperty _showSprite;
        private SerializedProperty _items;
        private SerializedProperty _customItems;

        private SerializedProperty _targetGraphic;
        private SerializedProperty _title;
        private SerializedProperty _image;
        
        private SerializedProperty _itemPrefab;
        private SerializedProperty _dropdownBackgroundPrefab;

        private SerializedProperty _onChanged;
        #endregion

        private void OnEnable()
        {
            // O'zgaruvchilarni ulash
            _spinnerType = serializedObject.FindProperty("spinnerType");
            _interactable = serializedObject.FindProperty("interactable");
            _animationDuration = serializedObject.FindProperty("animationDuration");

            _splitToRows = serializedObject.FindProperty("splitToRows");
            _rowSize = serializedObject.FindProperty("rowSize");
            _rowSpacing = serializedObject.FindProperty("rowSpacing");
            _padding = serializedObject.FindProperty("padding");
            _itemHeight = serializedObject.FindProperty("itemHeight");
            _itemWidth = serializedObject.FindProperty("itemWidth");
            _itemSpacing = serializedObject.FindProperty("itemSpacing");
            _selectedItemColor = serializedObject.FindProperty("selectedItemColor");
            _unselectedItemColor = serializedObject.FindProperty("unselectedItemColor");
            _selectedSecondaryColor = serializedObject.FindProperty("selectedSecondaryColor");
            _unselectedSecondaryColor = serializedObject.FindProperty("unselectedSecondaryColor"); 

            _showSprite = serializedObject.FindProperty("showSprite");
            _items = serializedObject.FindProperty("items");
            _customItems = serializedObject.FindProperty("customItems");

            _targetGraphic = serializedObject.FindProperty("spinnerImage");
            _title = serializedObject.FindProperty("title");
            _image = serializedObject.FindProperty("image");

            _itemPrefab = serializedObject.FindProperty("itemPrefab");
            _dropdownBackgroundPrefab = serializedObject.FindProperty("dropdownBackgroundPrefab");

            _onChanged = serializedObject.FindProperty("onChanged");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("General Configuration", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_spinnerType);
                EditorGUILayout.PropertyField(_interactable);
                EditorGUILayout.PropertyField(_animationDuration);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Layout & Spacing", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_itemWidth);
                EditorGUILayout.PropertyField(_itemHeight);
                EditorGUILayout.PropertyField(_padding);
                EditorGUILayout.PropertyField(_itemSpacing);
                EditorGUILayout.PropertyField(_selectedItemColor);
                EditorGUILayout.PropertyField(_unselectedItemColor);
                EditorGUILayout.PropertyField(_selectedSecondaryColor);
                EditorGUILayout.PropertyField(_unselectedSecondaryColor);

                if (_spinnerType.enumValueIndex == (int)SpinnerType.Dialog)
                {
                    EditorGUILayout.PropertyField(_splitToRows);
                    if (_splitToRows.boolValue)
                    {
                        EditorGUILayout.PropertyField(_rowSize);
                        EditorGUILayout.PropertyField(_rowSpacing);
                    }
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Content Data", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_showSprite);
                
                // ShowSprite holatiga qarab to'g'ri ro'yxatni ko'rsatish
                if (_showSprite.boolValue)
                {
                    EditorGUILayout.PropertyField(_customItems);
                }
                else
                {
                    EditorGUILayout.PropertyField(_items);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("UI References (Setup)", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_targetGraphic);
                EditorGUILayout.PropertyField(_title);
                
                // Image faqat sprite yoqilgan bo'lsa ko'rinadi
                if (_showSprite.boolValue)
                {
                    EditorGUILayout.PropertyField(_image);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Prefabs", EditorStyles.boldLabel);
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.PropertyField(_dropdownBackgroundPrefab);
                EditorGUILayout.PropertyField(_itemPrefab);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_onChanged);

            serializedObject.ApplyModifiedProperties();
        }
    }
}