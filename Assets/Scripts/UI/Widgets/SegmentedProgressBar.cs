using UnityEngine;
using UnityEngine.UI;

namespace UI.Widgets
{
    public class SegmentedProgressBar : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform containerRect;

        [Header("Settings")]
        [Range(1, 50)] [SerializeField] private int maxSegments = 5;

        [SerializeField] private Sprite segmentSprite;
        [SerializeField] private float segmentPixelsPerUnit = 1;
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color inactiveColor = Color.gray;
        [SerializeField] private float spacing = 5f;

        // Sahnada saqlanadigan (serialized) segmentlar massivi
        [HideInInspector] [SerializeField] private Image[] pooledSegments;

        /// <summary>
        /// O'yin vaqtida (Runtime) faqat shu funksiya ishlaydi - Chiqindi nol (Nol GC)
        /// </summary>
        public void UpdateProgress(int activeCount)
        {
            if (pooledSegments == null || pooledSegments.Length == 0) return;

            int currentActive = Mathf.Clamp(activeCount, 0, maxSegments);

            for (int i = 0; i < pooledSegments.Length; i++)
            {
                if (pooledSegments[i] != null)
                {
                    pooledSegments[i].color = (i < currentActive) ? activeColor : inactiveColor;
                }
            }
        }

#if UNITY_EDITOR
        /// <summary>
        /// Inspectorda biror qiymat o'zgarganda avtomatik ishlaydi (Faqat Editorda)
        /// </summary>
        private void OnValidate()
        {
            // Birinchi kadrda iyerarxiya band bo'lsa, kechiktirib chaqiramiz (Unity xatolik bermasligi uchun)
            UnityEditor.EditorApplication.delayCall += RebuildSegmentsInEditor;
        }

        private void RebuildSegmentsInEditor()
        {
            // Obyekt o'chirilgan bo'lsa, xatolik chiqmasligi uchun tekshiruv
            if (this == null || containerRect == null) return;

            // 1. Eskilarini tozalash (Iyerarxiyadan butkul o'chirish)
            for (int i = containerRect.childCount - 1; i >= 0; i--)
            {
                // Editorda Destroy o'rniga DestroyImmediate ishlatiladi
                DestroyImmediate(containerRect.GetChild(i).gameObject);
            }

            // 2. Yangi massivni tayyorlash
            pooledSegments = new Image[maxSegments];

            float totalWidth = containerRect.rect.width;
            float segmentWidth = (totalWidth - (spacing * (maxSegments - 1))) / maxSegments;

            // 3. Shablonni xotirada yaratish (Faqat ushbu sikl davomida ishlatish uchun)
            GameObject templateObj = new GameObject("Temp_Template", typeof(RectTransform), typeof(Image));
            Image templateImage = templateObj.GetComponent<Image>();
            templateImage.sprite = segmentSprite;
            templateImage.type = Image.Type.Sliced;
            templateImage.pixelsPerUnitMultiplier = segmentPixelsPerUnit;

            // 4. Segmentlarni ketma-ket yaratib joylashtirish
            for (int i = 0; i < maxSegments; i++)
            {
                Image newSegment = Instantiate(templateImage, containerRect, false);
                newSegment.name = $"Segment_{i}";
            
                RectTransform rect = newSegment.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0, 0.5f);
                rect.anchorMax = new Vector2(0, 0.5f);
                rect.pivot = new Vector2(0, 0.5f);

                float posX = i * (segmentWidth + spacing);
                rect.anchoredPosition = new Vector2(posX, 0);
                rect.sizeDelta = new Vector2(segmentWidth, containerRect.rect.height);

                newSegment.color = inactiveColor;
                pooledSegments[i] = newSegment;
            }

            // Vaqtincha yaratilgan shablonni o'chirib yuboramiz
            DestroyImmediate(templateObj);

            // 5. Eng muhim qismi: Sahnani va Obyektni "Dirty" (O'zgargan) deb belgilash
            // Bu Unity sahna saqlanganida (Ctrl+S) barcha yaratilgan segmentlarni faylga yozib qo'yishini ta'minlaydi
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.EditorUtility.SetDirty(containerRect);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }
#endif
    }
}