using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions; // JSON parsing uchun qo'shildi

namespace Editor.Localization
{
    public class LocalizationEditorWindow : EditorWindow
    {
        private string _folderPath = "Assets/StreamingAssets/Localization";
        private List<string> _languages = new();
        private Dictionary<string, Dictionary<string, string>> _masterData = new();
        
        // UI holatlari
        private string _newKey = "";
        private string _searchQuery = "";
        private string _selectedKey = "";
        
        // Yangi qo'shilgan o'zgaruvchilar (JSON Translator uchun)
        private bool _showJsonPanel = false;
        private string _jsonImportText = "";
        private Vector2 _jsonScrollPos;

        private Vector2 _leftScrollPos;
        private Vector2 _rightScrollPos;

        [MenuItem("Tools/Localization Editor")]
        public static void ShowWindow() => GetWindow<LocalizationEditorWindow>("Loc Editor");

        private void OnEnable() => LoadAllLanguages();

        private void LoadAllLanguages()
        {
            if (!Directory.Exists(_folderPath)) Directory.CreateDirectory(_folderPath);

            _masterData.Clear();
            _languages.Clear();
            _selectedKey = "";
            _showJsonPanel = false;

            string[] files = Directory.GetFiles(_folderPath, "*.json");
            foreach (var file in files)
            {
                string langCode = Path.GetFileNameWithoutExtension(file);
                _languages.Add(langCode);

                string json = File.ReadAllText(file);
                
                var serialization = JsonUtility.FromJson<Serialization<string, string>>(json);
                if (serialization != null)
                {
                    _masterData[langCode] = serialization.ToDictionary();
                }
                else
                {
                    _masterData[langCode] = new Dictionary<string, string>();
                }
            }
        }

        private void OnGUI()
        {
            // --- TOP BAR ---
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("Reload", EditorStyles.toolbarButton, GUILayout.Width(60))) LoadAllLanguages();
            GUILayout.Space(10);
            GUILayout.Label("Search:", GUILayout.Width(50));
            _searchQuery = EditorGUILayout.TextField(_searchQuery, EditorStyles.toolbarSearchField);
            EditorGUILayout.EndHorizontal();

            // --- MAIN SPLIT VIEW ---
            EditorGUILayout.BeginHorizontal();

            // ================= 1. LEFT PANEL (Keys List) =================
            EditorGUILayout.BeginVertical("box", GUILayout.Width(250));
            
            EditorGUILayout.BeginHorizontal();
            _newKey = EditorGUILayout.TextField(_newKey);
            if (GUILayout.Button("Add Key", GUILayout.Width(70))) AddKey(_newKey);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(5);

            _leftScrollPos = EditorGUILayout.BeginScrollView(_leftScrollPos);
            
            var allKeys = _masterData.Values.SelectMany(d => d.Keys).Distinct().ToList();
            var filteredKeys = string.IsNullOrEmpty(_searchQuery) 
                ? allKeys 
                : allKeys.Where(k => k.ToLower().Contains(_searchQuery.ToLower())).ToList();

            foreach (var key in filteredKeys)
            {
                bool isMissingTranslation = _languages.Any(lang => 
                    !_masterData[lang].ContainsKey(key) || string.IsNullOrWhiteSpace(_masterData[lang][key]));
                
                bool isSelected = (key == _selectedKey);
                Color originalColor = GUI.backgroundColor;
                
                if (isSelected) GUI.backgroundColor = new Color(0.3f, 0.7f, 1f);
                else if (isMissingTranslation) GUI.backgroundColor = Color.yellow;

                if (GUILayout.Button(key, EditorStyles.miniButtonLeft, GUILayout.Height(25)))
                {
                    _selectedKey = key;
                    _showJsonPanel = false; // Boshqa key tanlanganda JSON panelni yopamiz
                    GUI.FocusControl(null);
                }
                GUI.backgroundColor = originalColor;
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();


            // ================= 2. MIDDLE PANEL (Editor) =================
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandWidth(true));
            
            if (!string.IsNullOrEmpty(_selectedKey) && allKeys.Contains(_selectedKey))
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label($"Editing Key: {_selectedKey}", EditorStyles.largeLabel);
                
                // --- JSON tugmasi qo'shildi ---
                GUI.backgroundColor = _showJsonPanel ? Color.green : Color.white;
                if (GUILayout.Button("Get From Json", GUILayout.Width(120), GUILayout.Height(25)))
                {
                    _showJsonPanel = !_showJsonPanel;
                    if (_showJsonPanel) GenerateJsonTemplate();
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.Space();

                _rightScrollPos = EditorGUILayout.BeginScrollView(_rightScrollPos);

                foreach (var lang in _languages)
                {
                    if (!_masterData[lang].ContainsKey(_selectedKey)) _masterData[lang][_selectedKey] = "";

                    EditorGUILayout.LabelField(lang.ToUpper(), EditorStyles.boldLabel);
                    _masterData[lang][_selectedKey] = EditorGUILayout.TextArea(_masterData[lang][_selectedKey], GUILayout.MinHeight(40));
                    GUILayout.Space(10);
                }

                GUILayout.FlexibleSpace();
                
                GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
                if (GUILayout.Button("Delete This Key", GUILayout.Height(30)))
                {
                    RemoveKey(_selectedKey);
                    _selectedKey = "";
                    _showJsonPanel = false;
                }
                GUI.backgroundColor = Color.white;

                EditorGUILayout.EndScrollView();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Select a key from the left panel to edit its translations.", EditorStyles.centeredGreyMiniLabel);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.EndVertical();


            // ================= 3. RIGHT PANEL (JSON AI Translator) =================
            if (_showJsonPanel && !string.IsNullOrEmpty(_selectedKey))
            {
                EditorGUILayout.BeginVertical("box", GUILayout.Width(300));
                
                GUILayout.Label("AI Translator (JSON)", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox("1. Copy this JSON.\n2. Ask AI to translate values.\n3. Paste back here.\n4. Click Assign.", MessageType.Info);
                
                _jsonScrollPos = EditorGUILayout.BeginScrollView(_jsonScrollPos);
                // Matn maydoni (Katta hajmda)
                _jsonImportText = EditorGUILayout.TextArea(_jsonImportText, GUILayout.ExpandHeight(true));
                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = new Color(0.4f, 1f, 0.4f); // Yashil assign tugmasi
                if (GUILayout.Button("ASSIGN", GUILayout.Height(35))) AssignJson();
                
                GUI.backgroundColor = Color.white;
                if (GUILayout.Button("Close", GUILayout.Height(35), GUILayout.Width(60))) _showJsonPanel = false;
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal(); // END MAIN SPLIT VIEW

            // --- BOTTOM BAR ---
            if (GUILayout.Button("SAVE ALL LANGUAGES", GUILayout.Height(40))) SaveAll();
        }

        // ================= Yordamchi Funksiyalar (Logic) =================

        private void GenerateJsonTemplate()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            for (int i = 0; i < _languages.Count; i++)
            {
                string lang = _languages[i];
                string val = _masterData[lang][_selectedKey];
                
                // Matn ichidagi qo'shtirnoq va qatorlarni xavfsiz JSON formatiga o'tkazish
                val = val.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "");
                
                sb.Append($"  \"{lang}\": \"{val}\"");
                if (i < _languages.Count - 1) sb.AppendLine(",");
                else sb.AppendLine();
            }
            sb.AppendLine("}");
            _jsonImportText = sb.ToString();
            GUI.FocusControl(null); // Fokusni yangilash
        }

        private void AssignJson()
        {
            if (string.IsNullOrWhiteSpace(_jsonImportText)) return;

            try
            {
                // Regex: JSON ichidagi "key": "value" larni ishonchli ushlab olish
                string pattern = @"\""([a-zA-Z0-9_-]+)\""\s*:\s*\""([^\""\\]*(?:\\.[^\""\\]*)*)\""";
                MatchCollection matches = Regex.Matches(_jsonImportText, pattern, RegexOptions.Singleline);

                foreach (Match match in matches)
                {
                    string lang = match.Groups[1].Value;
                    string val = match.Groups[2].Value;

                    // AI qaytargan matndagi \n, \" kabi belgilarni toza matnga o'girish
                    val = Regex.Unescape(val);

                    if (_languages.Contains(lang) && _masterData.ContainsKey(lang))
                    {
                        _masterData[lang][_selectedKey] = val;
                    }
                }

                // Vazifa bajarildi, panelni yopamiz
                _showJsonPanel = false;
                GUI.FocusControl(null);
            }
            catch (System.Exception e)
            {
                Debug.LogError("JSON ni o'qishda xatolik yuz berdi. AI to'g'ri format berganiga ishonch hosil qiling. Xato: " + e.Message);
            }
        }

        private void AddKey(string key)
        {
            key = key.Trim();
            if (string.IsNullOrEmpty(key) || _languages.Count == 0) return;
            
            foreach (var lang in _languages)
            {
                if (!_masterData[lang].ContainsKey(key)) _masterData[lang].Add(key, "");
            }
            
            _newKey = "";
            _selectedKey = key;
            _showJsonPanel = false;
            GUI.FocusControl(null); 
        }

        private void RemoveKey(string key)
        {
            foreach (var lang in _languages) _masterData[lang].Remove(key);
        }

        private void SaveAll()
        {
            foreach (var lang in _languages)
            {
                string path = Path.Combine(_folderPath, lang + ".json");
                string json = JsonUtility.ToJson(new Serialization<string, string>(_masterData[lang]), true);
                File.WriteAllText(path, json);
            }
            AssetDatabase.Refresh();
            Debug.Log("Localization successfully saved to StreamingAssets.");
        }
    }

    [System.Serializable]
    public class Serialization<TKey, TValue>
    {
        public List<TKey> keys;
        public List<TValue> values;

        public Serialization(Dictionary<TKey, TValue> target)
        {
            keys = new List<TKey>(target.Keys);
            values = new List<TValue>(target.Values);
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            var result = new Dictionary<TKey, TValue>();
            for (int i = 0; i < keys.Count; i++) result.Add(keys[i], values[i]);
            return result;
        }
    }
}