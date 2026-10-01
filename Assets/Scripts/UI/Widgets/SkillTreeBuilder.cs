using System.Collections; // Coroutine uchun qo'shildi
using System.Collections.Generic;
using Core.Initialization;
using Data.Templates.Player;
using Core.Managers;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class SkillTreeBuilder : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private SkillNode rootNode;
        [SerializeField] private List<SkillNode> allNodes; 
        [SerializeField] private SkillView skillViewPrefab;
        [SerializeField] private RectTransform container; 

        [Header("Grid Settings")]
        [SerializeField] private Vector2 gridStepDistance = new Vector2(150f, 150f);

        [Header("Padding Settings")]
        [SerializeField] private Vector4 padding = new Vector4(100f, 100f, 100f, 100f); 

        [Header("Line Settings")]
        [SerializeField] private GameObject linePrefab; 
        [SerializeField] private Color lockedLineColor = Color.gray;
        [SerializeField] private Color unlockedLineColor = Color.cyan;
        [SerializeField] private float lineWidth = 5f;

        // DIQQAT: Kalit turi int dan string ga o'zgartirildi
        private Dictionary<string, SkillView> spawnedViews = new Dictionary<string, SkillView>();
        private List<GameObject> activeLines = new List<GameObject>();
        private Dictionary<SkillNode, List<SkillNode>> nodeRelationships = new Dictionary<SkillNode, List<SkillNode>>();

        // Tashqaridan container holatini boshqarish uchun property
        public GameObject ContainerObject => container != null ? container.gameObject : null;

        // Barcha jarayonni asinxron (Coroutine) bajaradigan metod
        public IEnumerator GenerateTreeRoutine()
        {
            // 1. Tozalash
            ClearTree();

            if (rootNode == null) yield break;

            MapRelationships();

            // 2. Tugunlarni vaqtincha (0,0) koordinatada yaratish
            BuildNode(rootNode, Vector2.zero);
    
            // Bir kadr kutamiz (Unity UI o'zini o'lchamlarini tushunib olishi va elementlar instantiating tugashi uchun)
            yield return null;

            // 3. Content o'lchamini hisoblab, tugunlarni joyiga surish
            FitContentAndCenter();

            // 4. Vizuallar va chiziqlarni chizish
            RefreshTreeVisuals();
            
            // UI chiziqlari ham kadr oxirida to'g'ri joylashishi uchun yana bir kadr kutamiz
            yield return null;
        }

        public void ClearTree()
        {
            foreach (Transform child in container) 
            {
                Destroy(child.gameObject);
            }
            spawnedViews.Clear();
            nodeRelationships.Clear();
            
            foreach (var line in activeLines)
            {
                if (line != null) Destroy(line);
            }
            activeLines.Clear();
        }

        private void MapRelationships()
        {
            foreach (var node in allNodes)
            {
                if (node.parentNode == null) continue;

                if (!nodeRelationships.ContainsKey(node.parentNode))
                {
                    nodeRelationships[node.parentNode] = new List<SkillNode>();
                }
                nodeRelationships[node.parentNode].Add(node);
            }
        }

        private void BuildNode(SkillNode node, Vector2 position)
        {
            if (node == null || string.IsNullOrEmpty(node.id) || spawnedViews.ContainsKey(node.id)) return;

            SkillView view = Instantiate(skillViewPrefab, container);
            view.skillNode = node;
            view.RectTransform.anchoredPosition = position;
            
            spawnedViews.Add(node.id, view);

            if (nodeRelationships.TryGetValue(node, out var children))
            {
                foreach (var child in children)
                {
                    Vector2 childPosition = position + CalculateOffset(child.directionFromParent);
                    BuildNode(child, childPosition);
                }
            }
        }

        private Vector2 CalculateOffset(AbilityNodeDirection direction)
        {
            return direction switch
            {
                AbilityNodeDirection.Top => new Vector2(0, gridStepDistance.y),
                AbilityNodeDirection.Bottom => new Vector2(0, -gridStepDistance.y),
                AbilityNodeDirection.Left => new Vector2(-gridStepDistance.x, 0),
                AbilityNodeDirection.Right => new Vector2(gridStepDistance.x, 0),
                _ => Vector2.zero
            };
        }

        private void FitContentAndCenter()
        {
            if (spawnedViews.Count == 0) return;

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            foreach (var view in spawnedViews.Values)
            {
                Vector2 pos = view.RectTransform.anchoredPosition;
                
                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;
            }

            float nodeWidth = skillViewPrefab.RectTransform.rect.width;
            float nodeHeight = skillViewPrefab.RectTransform.rect.height;

            float rawWidth = maxX - minX + nodeWidth;
            float rawHeight = maxY - minY + nodeHeight;

            float totalWidth = rawWidth + padding.x + padding.y; 
            float totalHeight = rawHeight + padding.z + padding.w; 

            container.sizeDelta = new Vector2(totalWidth, totalHeight);

            Vector2 pivotOffset = new Vector2(container.pivot.x * totalWidth, container.pivot.y * totalHeight);
            
            Vector2 shift = new Vector2(
                -minX + padding.x + (nodeWidth * 0.5f) - pivotOffset.x,
                -minY + padding.w + (nodeHeight * 0.5f) - pivotOffset.y
            );

            foreach (var view in spawnedViews.Values)
            {
                view.RectTransform.anchoredPosition += shift;
            }
        }

        public void RefreshTreeVisuals()
        {
            foreach (var view in spawnedViews.Values)
            {
                view.UpdateVisuals();
            }
            DrawConnections();

            // QO'SHILDI: Daraxt yangilanganda ochiq turgan ma'lumotlar panelini ham yangilash
            var fragmentView = GetComponentInParent<UI.Menu.Fragments.Skills.SkillsFragmentView>();
            if (fragmentView != null)
            {
                var detailPanel = fragmentView.GetComponentInChildren<UI.Menu.Panels.SelectedSkillPanel>(true);
                if (detailPanel != null && detailPanel.gameObject.activeSelf)
                {
                    detailPanel.UpdatePanelVisuals();
                }
            }
        }

        private void DrawConnections()
        {
            foreach (var line in activeLines)
            {
                if (line != null) Destroy(line);
            }
            activeLines.Clear();

            Canvas canvas = container.GetComponentInParent<Canvas>();
            float scaleFactor = canvas != null ? canvas.scaleFactor : 1f;

            foreach (var pair in spawnedViews)
            {
                SkillView view = pair.Value;
                if (view.skillNode == null || view.skillNode.parentNode == null) continue;

                // spawnedViews kaliti endi string bo'lgani uchun view.skillNode.parentNode.id (string) orqali tekshiriladi
                if (spawnedViews.TryGetValue(view.skillNode.parentNode.id, out SkillView parentView))
                {
                    bool isConnectionUnlocked = Application.isPlaying && 
                                                Hub.skillManager.IsSkillUnlocked(view.skillNode.id) && 
                                                Hub.skillManager.IsSkillUnlocked(parentView.skillNode.id);

                    CreateLine(parentView.RectTransform, view.RectTransform, isConnectionUnlocked, scaleFactor);
                }
            }
        }

        private void CreateLine(RectTransform startPos, RectTransform endPos, bool isUnlocked, float scaleFactor)
        {
            GameObject lineGo = Instantiate(linePrefab, container);
            lineGo.transform.SetAsFirstSibling(); 
            activeLines.Add(lineGo);

            Image lineImage = lineGo.GetComponent<Image>();
            lineImage.color = isUnlocked ? unlockedLineColor : lockedLineColor;

            RectTransform lineRect = lineGo.transform as RectTransform;
        
            Vector3 startPoint = startPos.position;
            Vector3 endPoint = endPos.position;

            Vector3 direction = endPoint - startPoint;
            float distance = direction.magnitude;

            lineRect.position = startPoint + direction * 0.5f;
            lineRect.right = direction.normalized;
        
            lineRect.sizeDelta = new Vector2(distance / scaleFactor, lineWidth);
        }
    }
}