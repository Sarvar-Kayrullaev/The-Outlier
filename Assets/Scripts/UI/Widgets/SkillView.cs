using Data.Templates.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core.Initialization;
using Core.Managers;
using UI.Menu.Fragments.Skills; // View ga ulanish uchun

namespace UI.Widgets
{
    public class SkillView : MonoBehaviour
    {
        public SkillNode skillNode;

        [Header("UI References")]
        [SerializeField] private Image iconImage;
        [SerializeField] private Button unlockButton; // Bu tugma endi skill ni Select qiluvchi asosiy tugma vazifasini bajaradi
        [SerializeField] private TMP_Text stateText;

        public RectTransform RectTransform => transform as RectTransform;

        private void Start()
        {
            if (skillNode != null) UpdateVisuals();
            
            // Diqqat: Endi u to'g'ridan-to'g'ri unlock qilmaydi, panelni ochadi
            unlockButton.onClick.AddListener(OnSkillClicked);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (skillNode == null) return;
            
            if (stateText != null) stateText.text = skillNode.skillName;
            if (iconImage != null) iconImage.sprite = skillNode.lockedIcon;

            gameObject.name = $"Skill: {skillNode.skillName} (ID: {skillNode.id})";
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif

        public void UpdateVisuals()
        {
            if (skillNode == null) return;

            bool isUnlocked = Hub.skillManager.IsSkillUnlocked(skillNode.id);
            bool canUnlock = skillNode.isRoot || (skillNode.parentNode != null && Hub.skillManager.IsSkillUnlocked(skillNode.parentNode.id));

            if (stateText != null) stateText.text = skillNode.skillName;

            // DIQQAT: Tugma har doim faol (interactable = true) bo'lishi kerak, 
            // chunki o'yinchi qulflangan skillni ham bosib descriptionini o'qiy olishi lozim.
            unlockButton.interactable = true; 

            if (isUnlocked)
            {
                if (iconImage != null) iconImage.sprite = skillNode.unlockedIcon;
            }
            else if (canUnlock)
            {
                if (iconImage != null) iconImage.sprite = skillNode.unlockableIcon;
            }
            else
            {
                if (iconImage != null) iconImage.sprite = skillNode.lockedIcon;
            }
        }

        private void OnSkillClicked()
        {
            if (skillNode == null) return;

            // Fragment View ni topamiz va unga ma'lumot yuboramiz
            SkillsFragmentView fragmentView = GetComponentInParent<SkillsFragmentView>();
            if (fragmentView != null)
            {
                fragmentView.ShowSkillDetails(skillNode);
            }
        }
    }
}