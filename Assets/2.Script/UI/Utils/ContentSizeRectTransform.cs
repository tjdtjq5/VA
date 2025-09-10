using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeRectTransform : MonoBehaviour
{
    [SerializeField] private float spacingX;
    [SerializeField] private float spacingY;
    
    [Button]
    public void SetFitHorizontal()  
    {
        float total = spacingX;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);

            if (child.GetComponent<ContentSizeFitter>())
            {
                child.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();
            }

            if (child.gameObject.activeSelf)
            {
                float sd = child.GetComponent<RectTransform>().sizeDelta.x;
                float scale = child.GetComponent<RectTransform>().localScale.x;
                // sd *= scale;
                total += sd;
            }
        }

        HorizontalLayoutGroup hlg = this.GetComponent<HorizontalLayoutGroup>();
        if (hlg != null)
        {
            total += hlg.spacing;
        }

        this.GetComponent<RectTransform>().sizeDelta = new Vector2(total, this.GetComponent<RectTransform>().sizeDelta.y);

        if (Application.isPlaying)
        {
            Managers.Tween.TweenInvoke(0.02f).SetOnComplete(() =>
            {
                InvokeLayout(true);
            });
        }
    }

    [Button]
    public void SetFitVertical()
    {
        float total = spacingY;
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            
            if (child.GetComponent<ContentSizeFitter>())
            {
                child.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            }

            if (child.gameObject.activeSelf)
            {
                float sd = child.GetComponent<RectTransform>().sizeDelta.y;
                float scale = child.GetComponent<RectTransform>().localScale.y;
                // sd *= scale;
                total += sd;
            }
        }

        VerticalLayoutGroup vlg = this.GetComponent<VerticalLayoutGroup>();
        if (vlg != null)
        {
            total += vlg.spacing;
        }
        
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x, total);

        if (Application.isPlaying)
        {
            Managers.Tween.TweenInvoke(0.02f).SetOnComplete(() =>
            {
                InvokeLayout(false);
            });
        }
    }

    private void InvokeLayout(bool isHorizontal)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) this.transform);

        if (isHorizontal)
        {
            HorizontalLayoutGroup hlg = this.GetComponent<HorizontalLayoutGroup>();
            if (hlg != null)
            {
                hlg.SetLayoutHorizontal();
            }
        }
        else
        {
            VerticalLayoutGroup vlg = this.GetComponent<VerticalLayoutGroup>();
            if (vlg != null)
            {
                vlg.SetLayoutVertical();
            }
        }
    }
}
