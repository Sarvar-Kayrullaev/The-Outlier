using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("Layout/Custom Horizontal Layout Group", 150)]
public class CustomHorizontalLayoutGroup : LayoutGroup
{
    [SerializeField] protected float m_Spacing = 0;
    public float spacing { get { return m_Spacing; } set { SetProperty(ref m_Spacing, value); } }

    [SerializeField] protected bool m_ChildControlWidth = true;
    public bool childControlWidth { get { return m_ChildControlWidth; } set { SetProperty(ref m_ChildControlWidth, value); } }

    [SerializeField] protected bool m_ChildControlHeight = true;
    public bool childControlHeight { get { return m_ChildControlHeight; } set { SetProperty(ref m_ChildControlHeight, value); } }

    [SerializeField] protected bool m_ChildForceExpandWidth = true;
    public bool childForceExpandWidth { get { return m_ChildForceExpandWidth; } set { SetProperty(ref m_ChildForceExpandWidth, value); } }

    [SerializeField] protected bool m_ChildForceExpandHeight = true;
    public bool childForceExpandHeight { get { return m_ChildForceExpandHeight; } set { SetProperty(ref m_ChildForceExpandHeight, value); } }

    [SerializeField] protected bool m_ChildScaleWidth = false;
    public bool childScaleWidth { get { return m_ChildScaleWidth; } set { SetProperty(ref m_ChildScaleWidth, value); } }

    [SerializeField] protected bool m_ChildScaleHeight = false;
    public bool childScaleHeight { get { return m_ChildScaleHeight; } set { SetProperty(ref m_ChildScaleHeight, value); } }

    [Header("Content Auto Fit Settings")]
    [SerializeField] protected bool m_UseContentPreferredWidth = false;
    public bool useContentPreferredWidth { get { return m_UseContentPreferredWidth; } set { SetProperty(ref m_UseContentPreferredWidth, value); } }

    [SerializeField] protected float m_MaxWidthLimit = 500f;
    public float maxWidthLimit { get { return m_MaxWidthLimit; } set { SetProperty(ref m_MaxWidthLimit, value); } }


    // Unity UI tizimi gorizontal o'lchamlarni hisoblashi uchun shart
    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();
        
        float totalMin = padding.horizontal;
        float totalPref = padding.horizontal;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            var child = rectChildren[i];
            float childPref = GetChildPreferredWidth(child);

            totalMin += childPref + (i > 0 ? spacing : 0);
            totalPref += childPref + (i > 0 ? spacing : 0);
        }

        SetLayoutInputForAxis(totalMin, totalPref, -1, 0);
    }

    // Unity UI tizimi vertikal o'lchamlarni hisoblashi uchun shart
    public override void CalculateLayoutInputVertical()
    {
        float totalMin = padding.vertical;
        float totalPref = padding.vertical;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            var child = rectChildren[i];
            totalMin = Mathf.Max(LayoutUtility.GetMinHeight(child) + padding.vertical, totalMin);
            totalPref = Mathf.Max(LayoutUtility.GetPreferredHeight(child) + padding.vertical, totalPref);
        }

        SetLayoutInputForAxis(totalMin, totalPref, -1, 1);
    }

    // Gorizontal joylashtirish mantiqi
    public override void SetLayoutHorizontal()
    {
        SetCellsAlongAxis(0);
    }

    // Vertikal joylashtirish mantiqi
    public override void SetLayoutVertical()
    {
        SetCellsAlongAxis(1);
    }

    // Bolaning haqiqiy kerakli kengligini aniqlash (TMP_Text hisobga olingan holda)
    private float GetChildPreferredWidth(RectTransform child)
    {
        float pref = LayoutUtility.GetPreferredWidth(child);

        if (m_UseContentPreferredWidth)
        {
            // Agar bolaning o'zida yoki uning ichki elementlarida TMP_Text yoki LayoutElement bo'lsa, 
            // Unity-ning LayoutUtility tizimi avtomatik ravishda eng ichki matn o'lchamigacha hisoblab beradi.
            if (pref <= 0)
            {
                pref = child.rect.width; 
            }

            // Maksimal chegara cheklovini qo'llash
            if (pref > m_MaxWidthLimit)
            {
                pref = m_MaxWidthLimit;
            }
        }

        return pref;
    }

    private void SetCellsAlongAxis(int axis)
    {
        if (axis == 0) // Horizontal Axis
        {
            float combinedPadding = padding.horizontal;
            float totalMinWidth = combinedPadding;
            float totalPreferredWidth = combinedPadding;
            float totalFlexibleWidth = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var child = rectChildren[i];
                float min = LayoutUtility.GetMinWidth(child);
                float pref = GetChildPreferredWidth(child);
                float flex = LayoutUtility.GetFlexibleWidth(child);

                if (childScaleWidth)
                {
                    float scaleX = child.localScale.x;
                    min *= scaleX;
                    pref *= scaleX;
                    flex *= scaleX;
                }

                totalMinWidth += min + (i > 0 ? spacing : 0);
                totalPreferredWidth += pref + (i > 0 ? spacing : 0);
                totalFlexibleWidth += flex;
            }

            if (childForceExpandWidth)
                totalFlexibleWidth = Mathf.Max(totalFlexibleWidth, 1);

            float totalSpace = rectTransform.rect.width;
            float surplusSpace = totalSpace - totalPreferredWidth;

            float currentX = padding.left;
            if (surplusSpace > 0 && totalFlexibleWidth == 0)
            {
                if (childAlignment == TextAnchor.UpperCenter || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.LowerCenter)
                    currentX += surplusSpace * 0.5f;
                else if (childAlignment == TextAnchor.UpperRight || childAlignment == TextAnchor.MiddleRight || childAlignment == TextAnchor.LowerRight)
                    currentX += surplusSpace;
            }

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var child = rectChildren[i];
                float min = LayoutUtility.GetMinWidth(child);
                float pref = GetChildPreferredWidth(child);
                float flex = LayoutUtility.GetFlexibleWidth(child);

                if (childScaleWidth)
                {
                    float scaleX = child.localScale.x;
                    min *= scaleX;
                    pref *= scaleX;
                    flex *= scaleX;
                }

                float childWidth = pref;
                if (childControlWidth)
                {
                    if (surplusSpace > 0 && totalFlexibleWidth > 0)
                    {
                        float factor = childForceExpandWidth ? 1f / rectChildren.Count : flex / totalFlexibleWidth;
                        childWidth += surplusSpace * factor;
                    }
                    else if (totalSpace < totalPreferredWidth && totalPreferredWidth - totalMinWidth > 0)
                    {
                        float shrinkFactor = (totalPreferredWidth - totalSpace) / (totalPreferredWidth - totalMinWidth);
                        childWidth -= (pref - min) * shrinkFactor;
                    }
                }

                // Agar avtomatik matn o'lchami yoqilgan bo'lsa va u limitdan oshsa, limitda qotirish
                if (m_UseContentPreferredWidth && childWidth > m_MaxWidthLimit)
                {
                    childWidth = m_MaxWidthLimit;
                }

                float pivotOffset = child.pivot.x * childWidth;
                if (childScaleWidth)
                    pivotOffset *= child.localScale.x;

                m_Tracker.Add(this, child,
                    DrivenTransformProperties.AnchoredPositionX |
                    DrivenTransformProperties.AnchorMinX |
                    DrivenTransformProperties.AnchorMaxX |
                    (childControlWidth ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.None));

                child.anchorMin = new Vector2(0, child.anchorMin.y);
                child.anchorMax = new Vector2(0, child.anchorMax.y);

                Vector2 pos = child.anchoredPosition;
                pos.x = currentX + pivotOffset;
                child.anchoredPosition = pos;

                if (childControlWidth)
                    child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, childWidth);

                currentX += childWidth + spacing;
            }
        }
        else // Vertical Axis
        {
            float totalHeight = rectTransform.rect.height;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var child = rectChildren[i];
                float min = LayoutUtility.GetMinHeight(child);
                float pref = LayoutUtility.GetPreferredHeight(child);
                float flex = LayoutUtility.GetFlexibleHeight(child);

                if (childScaleHeight)
                {
                    float scaleY = child.localScale.y;
                    min *= scaleY;
                    pref *= scaleY;
                    flex *= scaleY;
                }

                float childHeight = childControlHeight ? pref : child.rect.height;
                float availableHeight = totalHeight - padding.vertical;

                if (childControlHeight && childForceExpandHeight)
                    childHeight = availableHeight;
                else if (childControlHeight && flex > 0)
                    childHeight = Mathf.Max(min, availableHeight);

                float currentY = padding.top;
                if (childAlignment == TextAnchor.MiddleLeft || childAlignment == TextAnchor.MiddleCenter || childAlignment == TextAnchor.MiddleRight)
                    currentY = padding.top + (availableHeight - childHeight) * 0.5f;
                else if (childAlignment == TextAnchor.LowerLeft || childAlignment == TextAnchor.LowerCenter || childAlignment == TextAnchor.LowerRight)
                    currentY = padding.top + (availableHeight - childHeight);

                float pivotOffset = (1.0f - child.pivot.y) * childHeight;
                if (childScaleHeight)
                    pivotOffset *= child.localScale.y;

                m_Tracker.Add(this, child,
                    DrivenTransformProperties.AnchoredPositionY |
                    DrivenTransformProperties.AnchorMinY |
                    DrivenTransformProperties.AnchorMaxY |
                    (childControlHeight ? DrivenTransformProperties.SizeDeltaY : DrivenTransformProperties.None));

                child.anchorMin = new Vector2(child.anchorMin.x, 1);
                child.anchorMax = new Vector2(child.anchorMax.x, 1);

                Vector2 pos = child.anchoredPosition;
                pos.y = -currentY - pivotOffset;
                child.anchoredPosition = pos;

                if (childControlHeight)
                    child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, childHeight);
            }
        }
    }

    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        SetDirty();
    }
    #endif
}