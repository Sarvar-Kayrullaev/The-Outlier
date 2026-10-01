using UI.Widgets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Data.Templates.Player;
using Core.Initialization; // Hub uchun
using Core.Managers;

namespace UI.Menu.Panels
{
    public class SelectedSkillPanel : Panel
    {
        [Header("UI Component References")]
        [SerializeField] private Image referenceIcon;
        [SerializeField] private TMP_Text skillNameText;
        [SerializeField] private TMP_Text skillDescriptionText;
        [SerializeField] private TMP_Text costText;
        [SerializeField] private LiteButton unlockButton;

        [Header("Unlock Button Customization")] 
        [SerializeField] private Color interactableColor;
        [SerializeField] private Color nonInteractableColor;

        private SkillNode currentSkillNode;

        protected override void OnPanelShow()
        {
            // Panel ochilganda tugma bosilishini tinglaymiz
            if (unlockButton != null)
            {
                unlockButton.onClick.RemoveAllListeners();
                unlockButton.onClick.AddListener(OnUnlockButtonClicked);
            }
        }

        protected override void OnPanelHide()
        {
            // Panel yopilganda xotira tozalanishi uchun eventni yopamiz
            if (unlockButton != null)
            {
                unlockButton.onClick.RemoveAllListeners();
            }
            currentSkillNode = null;
        }

        /// <summary>
        /// Tashqaridan (View/Presenter dan) skill tanlanganda chaqiriladi
        /// </summary>
        public void SetupPanel(SkillNode node)
        {
            if (node == null) return;
            
            currentSkillNode = node;

            // Ma'lumotlarni UI elementlariga yozamiz
            if (skillNameText != null) skillNameText.text = node.skillName;
            
            // Diqqat: skillNode ichida description bor deb faraz qilamiz
            if (skillDescriptionText != null) skillDescriptionText.text = node.description; 

            UpdatePanelVisuals();
        }

        public void UpdatePanelVisuals()
        {
            if (currentSkillNode == null) return;

            bool isUnlocked = Hub.skillManager.IsSkillUnlocked(currentSkillNode.id);
            bool canUnlock = currentSkillNode.isRoot || 
                             (currentSkillNode.parentNode != null && Hub.skillManager.IsSkillUnlocked(currentSkillNode.parentNode.id));
            int currentSP = Hub.fundManager.GetSkillPoints();

            if (isUnlocked)
            {
                unlockButton.interactable = false;
                unlockButton.gameObject.SetActive(false);
                if (costText != null) costText.text = "Unlocked";
                if (referenceIcon != null) referenceIcon.sprite = currentSkillNode.unlockedIcon;
            }
            else if (canUnlock && currentSP >= currentSkillNode.cost)
            {
                unlockButton.interactable = true;
                unlockButton.gameObject.SetActive(true);
                unlockButton.primaryColor = interactableColor;
                unlockButton.hoverPrimaryColor = Color.white;
                unlockButton.secondaryColor = Color.white;
                unlockButton.hoverPrimaryColor = Color.black;
                if (costText != null) costText.text = $"Cost: {currentSkillNode.cost} SP";
                
                if (referenceIcon != null) referenceIcon.sprite = currentSkillNode.unlockableIcon;
            }
            else
            {
                // Yoki oldingi skill ochilmagan yoki SP yetarli emas
                unlockButton.interactable = false; 
                unlockButton.gameObject.SetActive(true);
                unlockButton.primaryColor = nonInteractableColor;
                unlockButton.hoverPrimaryColor = Color.white;
                unlockButton.secondaryColor = Color.white;
                unlockButton.hoverPrimaryColor = Color.black;
                if (canUnlock)
                {
                    if (referenceIcon != null) referenceIcon.sprite = currentSkillNode.unlockableIcon;
                    if (costText != null) costText.text = $"Cost: {currentSkillNode.cost} SP (Not enough SP)";
                }
                else
                {
                    if (referenceIcon != null) referenceIcon.sprite = currentSkillNode.lockedIcon;
                    if (costText != null) costText.text = "Locked: Pre-skill needed";
                }
            }
            unlockButton.UpdateVisualState();

        }

        private void OnUnlockButtonClicked()
        {
            if (currentSkillNode == null) return;

            // Backend orqali skill ochishga urinib ko'ramiz
            if (Hub.skillManager.TryUnlockSkill(currentSkillNode))
            {
                // Panel vizualini yangilaymiz
                UpdatePanelVisuals();

                // Butun skill daraxtini ham yangilash uchun global voqea yoki View orqali xabar beramiz
                // Bu yerda eng toza yo'l - daraxtni qayta chizishni buyurish
                var treeBuilder = Object.FindAnyObjectByType<SkillTreeBuilder>();
                if (treeBuilder != null)
                {
                    treeBuilder.RefreshTreeVisuals();
                }
            }
        }
    }
}