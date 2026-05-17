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
            foreach (var panel in allPanels) panel.Hide();
        }

        public void OpenPanel(Panel panel)
        {
            if (_uiStack.Count > 0)
            {
                // Agar oyna ochiq bo'lsa, u bilan ishlashni (raycast) vaqtincha yopish mumkin
                // _uiStack.Peek().Hide(); 
            }

            panel.Show();
            _uiStack.Push(panel);
        }

        public void CloseCurrentPanel()
        {
            if (_uiStack.Count == 0) return;

            Panel panel = _uiStack.Pop();
            panel.Hide();

            // Agar pastda yana oyna bo'lsa, uni faollashtirish
            if (_uiStack.Count > 0)
            {
                _uiStack.Peek().Show();
            }
        }
        
        public void OpenPanelByName(string panelName)
        {
            var panel = allPanels.FirstOrDefault(p => p.gameObject.name == panelName);
            if (panel != null) OpenPanel(panel);
        }
    }
}