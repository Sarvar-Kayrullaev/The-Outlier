using System;
using System.Collections.Generic;
using System.IO;
using Core.Initialization;
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
            SetLanguage(Hub.dataService.GetSettingsData().languageCode);
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

        public int GetIndexFromLanguageCode(string languageCode)
        {
            switch (languageCode.ToLower().Trim())
            {
                // === 1. TOP GLOBAL BOZORLAR ===
                case "en": return 0;  // English (AQSh / Buyuk Britaniya va global)
                case "zh": return 1;  // Chinese Simplified (Materik Xitoy)
                case "zt": return 2;  // Chinese Traditional (Tayvan / Gonkong)
                case "ru": return 3;  // Russian (Rossiya va MDH)
                case "pt": return 4;  // Portuguese (Braziliya va Portugaliya)
                case "es": return 5;  // Spanish (Meksika, Ispaniya va Lotin Amerikasi)
                case "de": return 6;  // German (Germaniya)
                case "fr": return 7;  // French (Fransiya)

                // === 2. OSIYO VA YAQIN SHARQ BLOKI ===
                case "ja": return 8;  // Japanese (Yaponiya)
                case "ko": return 9;  // Korean (Janubiy Koreya)
                case "id": return 10; // Indonesian (Indoneziya)
                case "vi": return 11; // Vietnamese (Vyetnam)
                case "th": return 12; // Thai (Tailand)
                case "tl": return 13; // Tagalog / Filipino (Filippin)
                case "ms": return 14; // Malay (Malayziya)
                case "tr": return 15; // Turkish (Turkiya)
                case "ar": return 16; // Arabic (Saudiya Arabistoni va Yaqin Sharq)

                // === 3. PREMIUM YEVROPA BLOKI ===
                case "it": return 17; // Italian (Italiya)
                case "pl": return 18; // Polish (Polsha)
                case "uk": return 19; // Ukrainian (Ukraina)
                case "nl": return 20; // Dutch (Niderlandiya)
                case "sv": return 21; // Swedish (Shvetsiya)
                case "cs": return 22; // Czech (Chexiya)
                case "el": return 23; // Greek (Gretsiya) -> Vengriya o'rniga qo'shildi

                // === 4. MAHALLIY SLOT ===
                case "uz": return 24; // Uzbek (O'zbekiston)

                // Default holatda ingliz tili (Xavfsizlik yostig'i)
                default: return 0;    
            }
        }
        public string GetLanguageCodeFromIndex(int index)
        {
            switch (index)
            {
                // === 1. TOP GLOBAL BOZORLAR ===
                case 0:  return "en"; // English (AQSh / Buyuk Britaniya va global)
                case 1:  return "zh"; // Chinese Simplified (Materik Xitoy)
                case 2:  return "zt"; // Chinese Traditional (Tayvan / Gonkong)
                case 3:  return "ru"; // Russian (Rossiya va MDH)
                case 4:  return "pt"; // Portuguese (Braziliya va Portugaliya)
                case 5:  return "es"; // Spanish (Meksika, Ispaniya va Lotin Amerikasi)
                case 6:  return "de"; // German (Germaniya)
                case 7:  return "fr"; // French (Fransiya)

                // === 2. OSIYO VA YAQIN SHARQ BLOKI ===
                case 8:  return "ja"; // Japanese (Yaponiya)
                case 9:  return "ko"; // Korean (Janubiy Koreya)
                case 10: return "id"; // Indonesian (Indoneziya)
                case 11: return "vi"; // Vietnamese (Vyetnam)
                case 12: return "th"; // Thai (Tailand)
                case 13: return "tl"; // Tagalog / Filipino (Filippin)
                case 14: return "ms"; // Malay (Malayziya)
                case 15: return "tr"; // Turkish (Turkiya)
                case 16: return "ar"; // Arabic (Saudiya Arabistoni va Yaqin Sharq)

                // === 3. PREMIUM YEVROPA BLOKI ===
                case 17: return "it"; // Italian (Italiya)
                case 18: return "pl"; // Polish (Polsha)
                case 19: return "uk"; // Ukrainian (Ukraina)
                case 20: return "nl"; // Dutch (Niderlandiya)
                case 21: return "sv"; // Swedish (Shvetsiya)
                case 22: return "cs"; // Czech (Chexiya)
                case 23: return "el"; // Greek (Gretsiya)

                // === 4. MAHALLIY SLOT ===
                case 24: return "uz"; // Uzbek (O'zbekiston)

                // Noto'g'ri indeks kelganda xavfsizlik yostig'i (Default: English)
                default: return "en"; 
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