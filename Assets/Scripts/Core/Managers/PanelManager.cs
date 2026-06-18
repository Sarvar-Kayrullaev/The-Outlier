using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UI.Widgets;
using UnityEngine;

namespace Core.Managers
{
    public class PanelManager : MonoBehaviour, ISingle
    {
        [SerializeField] private List<Panel> allPanels;
        private readonly Stack<Panel> _uiStack = new();

        private void Awake()
        {
            foreach (var panel in allPanels)
            {
                if (panel != null) panel.Hide();
            }
        }

        public void OpenPanel(Panel panel)
        {
            if (panel == null) return;

            // Agar panel allaqachon stack-da bo'lsa, qayta ochmaymiz
            if (_uiStack.Contains(panel)) return;

            // Optional: Agar xohlasangiz, yangi panel ochilganda eskisini vaqtincha berkitish
            if (_uiStack.Count > 0)
            {
                // _uiStack.Peek().Hide(); 
            }

            panel.Show();
            _uiStack.Push(panel);
        }

        public void CloseCurrentPanel()
        {
            // Himoya: Agar panellar bir freymda ketma-ket yopilmoqchi bo'lsa, to'xtatamiz
            Debug.Log("Close: "+_uiStack.Count);
            
            if (_uiStack.Count == 0) return;

            Panel panel = _uiStack.Pop();
            panel.Hide();
            Debug.Log("Panel Name: "+panel.name);

            // Agar pastda yana oyna bo'lsa, faqat o'shani ko'rsatamiz
            if (_uiStack.Count > 0)
            {
                _uiStack.Peek().Show();
            }
        }
        
        public void OpenPanelByName(string panelName)
        {
            var panel = allPanels.FirstOrDefault(p => p.gameObject.name == panelName);
            if (panel != null) 
            {
                OpenPanel(panel);
            }
            else
            {
                Debug.LogWarning($"PanelManager: {panelName} nomli panel topilmadi!");
            }
        }

        // Barcha panellarni majburiy yopish kerak bo'lgan holatlar uchun alohida funksiya
        public void CloseAllPanels()
        {
            while (_uiStack.Count > 0)
            {
                var panel = _uiStack.Pop();
                panel.Hide();
            }
        }
    }
}