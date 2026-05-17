using System;
using System.Collections.Generic;
using System.IO;
using Interfaces;
using UnityEngine;

namespace Core.Services
{
    public class LocalizationService : MonoBehaviour, ISingle
    {
        public event Action OnLanguageChanged;

        private Dictionary<string, string> _localizedText = new();
        private string _currentLanguage = "uz";

        private void Awake()
        {
            SetLanguage(_currentLanguage);
        }

        public void SetLanguage(string langCode)
        {
            _currentLanguage = langCode;
            LoadLanguageData(langCode);
            OnLanguageChanged?.Invoke();
        }

        private void LoadLanguageData(string langCode)
        {
            var path = Path.Combine(Application.streamingAssetsPath, "Localization", $"{langCode}.json");
            
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<Serialization<string, string>>(json);
                
                if (data != null)
                    _localizedText = data.ToDictionary();
            }
            else
            {
                Debug.LogWarning($"[LocalizationService] File not found: {path}");
            }
        }

        public string GetValue(string key)
        {
            return _localizedText.TryGetValue(key, out var value) ? value : $"[{key}]";
        }
        
        public string GetValue(string key, params object[] args)
        {
            var rawValue = GetValue(key);
            try 
            {
                return string.Format(rawValue, args);
            }
            catch 
            {
                return rawValue;
            }
        }
    }
    
    [Serializable]
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
            for (var i = 0; i < keys.Count; i++) 
            {
                if (!result.ContainsKey(keys[i]))
                    result.Add(keys[i], values[i]);
            }
            return result;
        }
    }
}