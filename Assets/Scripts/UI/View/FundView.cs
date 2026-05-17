using Core.Managers;
using TMPro;
using UnityEngine;

namespace UI.View
{
    public class FundView
    {
        [SerializeField] private TMP_Text balanceText;
        [SerializeField] private TMP_Text skillPointText;

        private void OnEnable()
        {
            FundManager.OnBalanceChanged += UpdateBalanceUI;
            FundManager.OnSkillPointChanged += UpdateSkillPointUI;
        }

        private void OnDisable()
        {
            FundManager.OnBalanceChanged -= UpdateBalanceUI;
            FundManager.OnSkillPointChanged -= UpdateSkillPointUI;
        }

        private void UpdateBalanceUI(int currentBalance)
        {
            balanceText.text = currentBalance.ToString("N0");
        }
        private void UpdateSkillPointUI(int currentSkillPoint)
        {
            skillPointText.text = currentSkillPoint.ToString("N0");
        }
    }
}