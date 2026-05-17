using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Widgets
{
    public class TabView : MonoBehaviour
    {
        [Header("UI Containers")]
        [SerializeField] public RectTransform tabParent;
        [SerializeField] public RectTransform fragmentParent;

        [Header("Customization")]
        [SerializeField] private Color selectedTextColor = Color.white;
        [SerializeField] private Color unselectedTextColor = Color.gray;
        
        // These will be auto-assigned by the Editor script
        [HideInInspector] public List<TabButtonView> tabButtonViews = new();
        [HideInInspector] public List<TabFragmentView> fragmentViews = new();

        public UnityEvent<int> OnChanged;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var fragment in fragmentViews)
            {
                if (fragment != null) fragment.OnFragmentInit();
            }

            for (int i = 0; i < tabButtonViews.Count; i++)
            {
                int index = i;
                tabButtonViews[i].OnClick.RemoveAllListeners();
                tabButtonViews[i].OnClick.AddListener(() => Select(index));
            }
            Select(0);
        }

        public void Select(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex >= tabButtonViews.Count) return;

            for (int i = 0; i < tabButtonViews.Count; i++)
            {
                bool isSelected = (i == selectedIndex);
        
                tabButtonViews[i].SetSelection(isSelected, isSelected ? selectedTextColor : unselectedTextColor);
        
                if (i < fragmentViews.Count && fragmentViews[i] != null)
                {
                    fragmentViews[i].gameObject.SetActive(isSelected);

                    if (isSelected)
                    {
                        fragmentViews[i].OnFragmentSelected();
                    }
                    else
                    {
                        fragmentViews[i].OnFragmentDeactivated();
                    }
                }
            }
            OnChanged?.Invoke(selectedIndex);
        }
    }
}