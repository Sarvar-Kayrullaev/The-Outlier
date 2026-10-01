using System.Collections;
using Core.Initialization;
using UnityEngine;
using UI.Widgets;
using UI.Menu.Panels; // Panel komponenti uchun qo'shildi

namespace UI.Menu.Fragments.Skills
{
    public class SkillsFragmentView : TabFragmentView
    {
        public SkillTreeBuilder skillTreeBuilder;
        
        // Yangi qo'shilgan qism: Tanlangan skill ma'lumotlar paneli reference'i
        [Header("Detail Panel")]
        [SerializeField] private SelectedSkillPanel selectedSkillPanel;

        [Header("Loading UI")]
        [SerializeField] private GameObject loadingPanel;

        private Coroutine buildCoroutine;

        public override void OnFragmentInit()
        {
            if (loadingPanel != null) loadingPanel.SetActive(false);
            if (selectedSkillPanel != null) selectedSkillPanel.Hide(); // Boshida berkitamiz
        }

        public override void OnFragmentSelected()
        {
            if (buildCoroutine != null)
            {
                StopCoroutine(buildCoroutine);
            }
            
            buildCoroutine = StartCoroutine(BuildSkillTreeSequence());
        }

        public override void OnFragmentDeactivated()
        {
            if (buildCoroutine != null)
            {
                StopCoroutine(buildCoroutine);
                buildCoroutine = null;
            }

            if (skillTreeBuilder != null)
            {
                skillTreeBuilder.ClearTree();
                skillTreeBuilder.ContainerObject?.SetActive(false);
            }

            if (loadingPanel != null)
            {
                loadingPanel.SetActive(false);
            }

            // Fragment yopilganda yon panelni ham yopamiz
            if (selectedSkillPanel != null)
            {
                selectedSkillPanel.Hide();
            }
        }

        private IEnumerator BuildSkillTreeSequence()
        {
            if (skillTreeBuilder == null) yield break;

            if (loadingPanel != null) loadingPanel.SetActive(true);
            skillTreeBuilder.ContainerObject?.SetActive(false);

            yield return StartCoroutine(skillTreeBuilder.GenerateTreeRoutine());

            skillTreeBuilder.ContainerObject?.SetActive(true);
            if (loadingPanel != null) loadingPanel.SetActive(false);

            buildCoroutine = null;
        }

        // Skill bosilganda chaqiriladigan ko'prik funksiya
        public void ShowSkillDetails(Data.Templates.Player.SkillNode node)
        {
            if (selectedSkillPanel != null)
            {
                Hub.panelManager.OpenPanel(selectedSkillPanel);
                selectedSkillPanel.SetupPanel(node);
            }
        }
    }
}