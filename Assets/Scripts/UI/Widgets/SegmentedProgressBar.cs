using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    /// <summary>
    /// A visual segmented progress indicator used to display static progress like completed missions or captured outposts.
    /// This component is read-only from the UI side and receives value updates strictly from external scripts.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class SegmentedProgressBar : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform containerRect;

        [Header("Segment Settings")]
        [Range(1, 50)] [SerializeField] private int maxSegments = 5;
        [SerializeField] private float spacing = 5f;
        
        [Header("Visual Styling")]
        [SerializeField] private Sprite segmentSprite;
        [Tooltip("Type used when sprite is assigned. If sprite is null, type automatically reverts to Simple.")]
        [SerializeField] private Image.Type imageType = Image.Type.Sliced;
        [SerializeField] private float pixelsPerUnitMultiplier = 1f;
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color inactiveColor = Color.gray;

        // Serialized array to cache generated segment references within the scene
        [HideInInspector] [SerializeField] private Image[] pooledSegments;
        
        private int currentActiveCount = 0;

        /// <summary>
        /// Gets or sets the current active segments. Updates the UI automatically when changed.
        /// </summary>
        public int CurrentActiveSegments
        {
            get => currentActiveCount;
            set => UpdateProgress(value);
        }

        private void Start()
        {
            if (containerRect == null)
            {
                containerRect = GetComponent<RectTransform>();
            }
            
            UpdateProgress(currentActiveCount);
        }

        /// <summary>
        /// Updates the visual state of the segments. Zero Garbage Collection (Zero GC) at runtime.
        /// </summary>
        public void UpdateProgress(int activeCount)
        {
            currentActiveCount = Mathf.Clamp(activeCount, 0, maxSegments);

            if (pooledSegments == null || pooledSegments.Length == 0) return;

            for (int i = 0; i < pooledSegments.Length; i++)
            {
                if (pooledSegments[i] != null)
                {
                    pooledSegments[i].color = (i < currentActiveCount) ? activeColor : inactiveColor;
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Automatically triggered in the Unity Editor whenever a property value changes in the Inspector.
        /// </summary>
        private void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall -= RebuildSegmentsInEditor;
            UnityEditor.EditorApplication.delayCall += RebuildSegmentsInEditor;
        }

        /// <summary>
        /// Rebuilds the entire layout framework and instantiates segments inside the Editor scene view safely.
        /// </summary>
        private void RebuildSegmentsInEditor()
        {
            UnityEditor.EditorApplication.delayCall -= RebuildSegmentsInEditor;
            if (Application.isPlaying || this == null || containerRect == null) return;

            // FIX 1: Prevent modification if this object is a persistent Prefab asset in the Project folders
            if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(this)) return;

            // 1. Setup or update the HorizontalLayoutGroup for clean automatic layout distribution
            HorizontalLayoutGroup layoutGroup = containerRect.GetComponent<HorizontalLayoutGroup>();
            if (layoutGroup == null)
            {
                layoutGroup = containerRect.gameObject.AddComponent<HorizontalLayoutGroup>();
            }
            
            layoutGroup.spacing = spacing;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;

            // 2. Clear existing old segments from the hierarchy safely using Undo tracking
            for (int i = containerRect.childCount - 1; i >= 0; i--)
            {
                GameObject child = containerRect.GetChild(i).gameObject;
                
                // FIX 2: Use UnityEditor.Undo to handle deletion safely without triggering asset loss warnings
                UnityEditor.Undo.DestroyObjectImmediate(child);
            }

            // 3. Initialize the references array
            pooledSegments = new Image[maxSegments];

            // 4. Create a temporary setup configuration
            GameObject templateObj = new GameObject("Temp_Template", typeof(RectTransform), typeof(Image));
            Image templateImage = templateObj.GetComponent<Image>();
            
            templateImage.sprite = segmentSprite;
            templateImage.type = (segmentSprite == null) ? Image.Type.Simple : imageType;
            templateImage.pixelsPerUnitMultiplier = pixelsPerUnitMultiplier;

            // 5. Instantiate new segments according to maxSegments settings
            for (int i = 0; i < maxSegments; i++)
            {
                Image newSegment = Instantiate(templateImage, containerRect, false);
                newSegment.name = $"Segment_{i}";
                newSegment.color = (i < currentActiveCount) ? activeColor : inactiveColor;
                
                // FIX 3: Register created objects to Undo system so Unity officially recognizes them in the scene structure
                UnityEditor.Undo.RegisterCreatedObjectUndo(newSegment.gameObject, "Create Segment");
                
                pooledSegments[i] = newSegment;
            }

            // Discard the temporary template setup
            DestroyImmediate(templateObj);

            // 6. Force layout calculations to refresh instantly inside the Editor window
            Canvas.ForceUpdateCanvases();
            layoutGroup.CalculateLayoutInputHorizontal();
            layoutGroup.CalculateLayoutInputVertical();
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();

            // 7. Mark objects as dirty to force Unity to save the newly created hierarchy structure to the scene file
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(containerRect);
            if (gameObject.scene.IsValid())
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
        }
#endif
    }
}