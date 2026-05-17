using Core.Initialization;
using TMPro;
using UnityEngine;

namespace UI.Widgets
{
    [RequireComponent(typeof(TMP_Text))]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField, HideInInspector] private TMP_Text _textComponent; // Editorda ko'rinmaydi, lekin saqlanadi

        private void OnValidate()
        {
            if (_textComponent == null)
            {
                _textComponent = GetComponent<TMP_Text>();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        private void OnEnable()
        {
            Hub.localizationService.OnLanguageChanged += UpdateText;
            UpdateText();
        }

        private void OnDisable()
        {
            if (Hub.localizationService != null)
                Hub.localizationService.OnLanguageChanged -= UpdateText;
        }

        private void UpdateText()
        {
            if (Hub.localizationService != null)
                _textComponent.text = Hub.localizationService.GetValue(key);
        }

        public void SetText(string textKey)
        {
            key = textKey;
            UpdateText();
        }
    }
}